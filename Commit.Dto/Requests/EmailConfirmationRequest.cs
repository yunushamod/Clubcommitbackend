using System.ComponentModel.DataAnnotations;
namespace Commit.Dto.Requests;

public class EmailConfirmationRequest : EmailRequest
{
    [Required, Length(6, 6)]
    public required string Code { get; set; }
}