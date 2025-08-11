using AutoMapper;
using Commit.Data;
using Commit.Dto.Requests;
using Commit.Dto.Responses;
using Commit.Services.Interfaces;
namespace Commit.Services.Implementations;

public class AuthenticationService(ApplicationDbContext context, IMapper mapper) : IAuthenticationService
{
    public async Task<Result<bool>> Signup(UserSignUpRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public async Task<Result<SignInResponse>> SignIn(SignInRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public async Task<Result<bool>> GenerateEmailConfirmationToken(EmailRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public async Task<Result<bool>> GeneratePasswordResetConfirmationToken(EmailRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public async Task<Result<bool>> ConfirmPasswordResetToken(EmailConfirmationRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public async Task<Result<bool>> ConfirmEmailToken(EmailConfirmationRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public async Task<Result<bool>> ResetPassword(PasswordResetRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    public async Task<Result<bool>> ChangePassword(PasswordChangeRequest request, CancellationToken cancellationToken = default) => throw new NotImplementedException();
}