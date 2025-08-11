using System.ComponentModel.DataAnnotations;
namespace Commit.Dto.Requests;

public class EmailRequest
{
    [Required, EmailAddress]
    public required string EmailAddress { get; init; }
}