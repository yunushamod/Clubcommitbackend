using System.ComponentModel.DataAnnotations;
namespace Commit.Data.Models;

public class Otp : BaseModel
{
    [MaxLength(350)]
    public required string Email { get; init; }
    [MaxLength(10)]
    public required string Value { get; init; }
    public bool Validated { get; set; }
    public required DateTime ValidUntil { get; init; }
    public required OtpType Type { get; init; }

    public bool IsValid()
    {
        return !Validated && ValidUntil >= DateTime.UtcNow.AddHours(1);
    }
}