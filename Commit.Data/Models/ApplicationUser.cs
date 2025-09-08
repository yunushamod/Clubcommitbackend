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
    [MaxLength(100)]
    public required string PasswordHash { get; set; }
    [Required, MaxLength(300)]
    public required string Location { get; init; }
    [MaxLength(300)]
    public string? ProfileImageUrl { get; init; }
    [Required]
    public required decimal Latitude { get; init; }
    [Required]
    public required decimal Longitude { get; init; }
    public required Gender Gender { get; init; }
    [Required, MaxLength(100)]
    public required string Audience { get; init; }
    public DateTime? LastLogin { get; set; }
    [MaxLength(100)]
    public required string SecurityStamp { get; set; }
    public DateTime? LastPasswordReset { get; set; }
    public DateTime? LastPasswordChange { get; set; }
    public bool CanChangePassword { get; set; }
}