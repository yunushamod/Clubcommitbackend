using Commit.Dto.Requests;
using Commit.Dto.Responses;
namespace Commit.Services.Interfaces;

public interface IAuthenticationService
{
    Task<Result<bool>> Signup(UserSignUpRequest request, CancellationToken cancellationToken = default);
    Task<Result<SignInResponse>> SignIn(SignInRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> GenerateEmailConfirmationToken(EmailRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> GeneratePasswordResetConfirmationToken(EmailRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> ConfirmPasswordResetToken(EmailConfirmationRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> ConfirmEmailToken(EmailConfirmationRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> ResetPassword(PasswordResetRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> ChangePassword(PasswordChangeRequest request, CancellationToken cancellationToken = default);
}