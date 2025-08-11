using Commit.Api.Extensions;
using Commit.Dto.Requests;
using Commit.Dto.Responses;
using Commit.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Commit.Api.Controllers;

public class AuthenticationController(IAuthenticationService authenticationService) : BaseController
{
    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<Result<bool>>> Signup(UserSignUpRequest request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.Signup(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<Result<SignInResponse>>> SignIn(SignInRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.SignIn(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<Result<bool>>> GenerateEmailConfirmationToken(EmailRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.GenerateEmailConfirmationToken(request, cancellationToken);
        return result.ToActionResult();
    }
    
    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<Result<bool>>> GeneratePasswordResetConfirmationToken(EmailRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.GeneratePasswordResetConfirmationToken(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<Result<bool>>> ConfirmPasswordResetToken(EmailConfirmationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.ConfirmPasswordResetToken(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<Result<bool>>> ConfirmEmailToken(EmailConfirmationRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.ConfirmEmailToken(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost, AllowAnonymous]
    public async Task<ActionResult<Result<bool>>> ResetPassword(PasswordResetRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.ResetPassword(request, cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<Result<bool>>> ChangePassword(PasswordChangeRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authenticationService.ChangePassword(request, cancellationToken);
        return result.ToActionResult();
    }
}