using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Commit.Data;
using Commit.Data.DbExtensions;
using Commit.Data.Models;
using Commit.Dto.Requests;
using Commit.Dto.Responses;
using Commit.Services.Extensions;
using Commit.Services.Helpers;
using Commit.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace Commit.Services.Implementations;

public class AuthenticationService(ApplicationDbContext context, IMapper mapper,
    ILogger<AuthenticationService> logger,
    IPasswordHasher<ApplicationUser> passwordHasher,
    IOptions<JwtSettings> jwtSettings,
    IHttpContextAccessor httpContextAccessor) : IAuthenticationService
{
    private readonly JwtSettings jwtSettings = jwtSettings.Value;
    public async Task<Result<bool>> Signup(UserSignUpRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = mapper.Map<ApplicationUser>(request);
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            // TODO: Send an email
            return Result<bool>.Ok(true);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "An error occurred while trying to create user");
            return Result<bool>.Failed(ex.GetUniqueConstraintViolationMessage());
        }
    }
    public async Task<Result<SignInResponse>> SignIn(SignInRequest request, CancellationToken cancellationToken = default)
    {
        async Task<SignInResponse> GetSignInResult(ApplicationUser user)
        {
            user.LastLogin = DateTime.UtcNow.AddHours(1);
            await context.SaveChangesAsync(cancellationToken);
            var accessToken = CreateToken(user);
            var result = new SignInResponse
            {
                User = mapper.Map<ApplicationUserResponse>(user),
                AccessToken = accessToken,
            };
            return result;
        }
        var user = await context.ApplicationUsers.SingleOrDefaultAsync(x =>
            x.NormalizedEmailAddress == request.EmailAddress.Trim().ToLower(), cancellationToken);
        if(user is null) return Result<SignInResponse>.Unauthorized("Invalid email/password");
        var passwordResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        switch (passwordResult)
        {
            case PasswordVerificationResult.SuccessRehashNeeded:
                user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
                return Result<SignInResponse>.Ok(await GetSignInResult(user));
            case PasswordVerificationResult.Success:
                return Result<SignInResponse>.Ok(await GetSignInResult(user));
            case PasswordVerificationResult.Failed:
            default:
                return Result<SignInResponse>.Unauthorized("Invalid username/password");
        }
    }
    public async Task<Result<bool>> GenerateEmailConfirmationToken(EmailRequest request, CancellationToken cancellationToken = default)
    {
        var otpString = Utils.GenerateRandomOtp(6);
        var otp = new Otp
        {
            Type = OtpType.SignUp,
            Email = request.EmailAddress.Trim().ToLower(),
            Value = otpString,
            ValidUntil = DateTime.UtcNow.AddHours(1).AddMinutes(5)
        };
        await context.AddAsync(otp, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        // var emailTemplate = EmailTemplate.ConfirmEmailAddress;
        // await emailService.SendEmailAsync(user.Email, user.FullName,
        // emailTemplate.Subject, emailTemplate.Content.Replace("{{OTP}}", otp.OtpString));
        return Result<bool>.Ok(true);
    }
    public async Task<Result<bool>> GeneratePasswordResetConfirmationToken(EmailRequest request, CancellationToken cancellationToken = default)
    {
        var user = await context.ApplicationUsers.SingleOrDefaultAsync(x => x.NormalizedEmailAddress == request
            .EmailAddress.ToLower().Trim(), cancellationToken);
        if(user is null) return Result<bool>.Ok(true);
        var otpString = Utils.GenerateRandomOtp(6);
        var otp = new Otp
        {
            Type = OtpType.PasswordReset,
            Email = request.EmailAddress.Trim().ToLower(),
            Value = otpString,
            ValidUntil = DateTime.UtcNow.AddHours(1).AddMinutes(5)
        };
        await context.AddAsync(otp, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        // var emailTemplate = EmailTemplate.GeneratePasswordResetToken;
        // await emailService.SendEmailAsync(user.EmailAddress, user.FirstName, emailTemplate.Subject, emailTemplate.Content
        //     .Replace("{{OTP}}", otpString), cancellationToken: cancellationToken);
        return Result<bool>.Ok(true);
    }
    public async Task<Result<bool>> ConfirmPasswordResetToken(EmailConfirmationRequest request, CancellationToken cancellationToken = default)
    {
        var otp = await context.Otp.SingleOrDefaultAsync(x => x.Email == request.EmailAddress
                                                                  .Trim().ToLower()
                                                              && x.Value == request.Code && x.Type == OtpType.PasswordReset, cancellationToken);
        if(otp is null) return Result<bool>.Failed("Invalid OTP");
        if(!otp.IsValid()) return Result<bool>.Failed("OTP has expired. Kindly generate a new one");
        otp.Validated = true;
        otp.DateModified = DateTime.UtcNow.AddHours(1);
        var user = await GetUserFromToken(otp.Email);
        if(user == null) return Result<bool>.Failed("User record not found");
        user.CanChangePassword = true;
        await context.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok(true);
    }
    
    private async Task<ApplicationUser?> GetUserFromToken(string key)
    {
        return await context.ApplicationUsers.SingleOrDefaultAsync(x => x.NormalizedEmailAddress == key.Trim().ToLower());
    }
    public async Task<Result<bool>> ConfirmEmailToken(EmailConfirmationRequest request, CancellationToken cancellationToken = default)
    {
        var otp = await context.Otp.SingleOrDefaultAsync(x => x.Email == request.EmailAddress
                                                                  .Trim().ToLower()
                                                              && x.Value == request.Code && x.Type == OtpType.SignUp, cancellationToken);
        if(otp is null) return Result<bool>.Failed("Invalid OTP");
        if(!otp.IsValid()) return Result<bool>.Failed("OTP has expired. Kindly generate a new one");
        otp.Validated = true;
        otp.DateModified = DateTime.UtcNow.AddHours(1);
        await context.SaveChangesAsync(cancellationToken);
        return Result<bool>.Ok(true);
    }
    public async Task<Result<bool>> ResetPassword(PasswordResetRequest request, CancellationToken cancellationToken = default)
    {
        var user = await context.ApplicationUsers.SingleOrDefaultAsync(x => 
            x.NormalizedEmailAddress == request.EmailAddress.ToLower(), cancellationToken);
        if(user is null) return Result<bool>.Failed("User record not found");
        if(!user.CanChangePassword) return Result<bool>.Failed();
        user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);
        user.DateModified = DateTime.UtcNow.AddHours(1);
        user.LastPasswordReset = DateTime.UtcNow.AddHours(1);
        user.CanChangePassword = false;
        await context.SaveChangesAsync(cancellationToken);
        // var emailTemplate = EmailTemplate.PasswordReset;
        // await emailService.SendEmailAsync(user.EmailAddress, user.FirstName, emailTemplate.Subject, 
        //     emailTemplate.Content, cancellationToken: cancellationToken);
        return Result<bool>.Ok(true);
    }
    public async Task<Result<bool>> ChangePassword(PasswordChangeRequest request, CancellationToken cancellationToken = default)
    {
        var user = await context.ApplicationUsers.SingleOrDefaultAsync(x => 
            x.Id == httpContextAccessor.GetUserId(), cancellationToken);
        if(user is null) return Result<bool>.Unauthorized();
        var passResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);
        if(passResult == PasswordVerificationResult.Failed) return Result<bool>.Failed("Invalid password");
        user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);
        await context.SaveChangesAsync(cancellationToken);
        // var emailTemplate = EmailTemplate.ChangePassword;
        // await emailService.SendEmailAsync(user.EmailAddress, user.FirstName, emailTemplate.Subject, emailTemplate.Content, 
        //     cancellationToken: cancellationToken);
        return Result<bool>.Ok(true);
    }
    
    
    private string CreateToken(ApplicationUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return accessToken;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(jwtSettings.Secret);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private static IEnumerable<Claim> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.EmailAddress),
            new(ClaimTypes.PrimarySid, user.Id.ToString()),
        };
        return claims;
    }

    private SecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings.ValidIssuer,
            audience: jwtSettings.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings.Expires)),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }
}