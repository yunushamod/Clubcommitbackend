namespace Commit.Dto.Requests;

public class PasswordResetRequest : EmailRequest
{
    public required string NewPassword { get; init; }
}