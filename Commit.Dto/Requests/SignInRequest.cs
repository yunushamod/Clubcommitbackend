using System.ComponentModel.DataAnnotations;
namespace Commit.Dto.Requests;

public class SignInRequest : EmailRequest
{
    [Required]
    public required string Password { get; init; }
}