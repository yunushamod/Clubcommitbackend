using System.ComponentModel.DataAnnotations;
namespace Commit.Data.Models;

public class ApplicationUser : BaseModel
{
    [MaxLength(255)]
    public required string FirstName { get; set; }
    [MaxLength(255)]
    public required string LastName { get; set; }
    [MaxLength(300)]
    public required string EmailAddress { get; set; }
    [MaxLength(300)]
    public required string NormalizedEmailAddress { get; set; }
    [MaxLength(15)]
    public required string PhoneNumber { get; set; }
    [MaxLength(15)]
    public required string NormalizedPhoneNumber { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime? LastLogin { get; set; }
    public required string SecurityStamp { get; set; }
    public DateTime? LastPasswordReset { get; set; }
    public DateTime? LastPasswordChange { get; set; }
}