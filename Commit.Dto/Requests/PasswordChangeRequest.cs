using System.ComponentModel.DataAnnotations;
namespace Commit.Dto.Requests;

public class PasswordChangeRequest : EmailRequest
{
    [Required]
    public required string CurrentPassword { get; init; }
    [Required, MinLength(8)]
    public required string NewPassword { get; init; }
}