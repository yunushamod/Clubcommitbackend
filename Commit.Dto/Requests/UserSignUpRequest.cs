using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AutoMapper;
using Commit.Data.Models;
namespace Commit.Dto.Requests;

[AutoMap(typeof(ApplicationUser), ReverseMap = true)]
public class UserSignUpRequest : EmailRequest
{
    [Required, MaxLength(255)]
    public required string FirstName { get; init; }
    [Required, MaxLength(255)]
    public required string LastName { get; init; }
    [JsonIgnore]
    public string NormalizedEmailAddress => EmailAddress.Trim().ToLower();
    [Required, Phone]
    public required string PhoneNumber { get; init; }
    [JsonIgnore]
    public string NormalizedPhoneNumber => PhoneNumber.Trim().TrimStart('+');
    [Required, RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
     ErrorMessage = "Password must be at least 8 characters long, and contain at least one uppercase letter, one lowercase letter, one number, and one special character."), 
     DataType(DataType.Password)]
    public required string Password { get; init; }
    [Required, MaxLength(300)]
    public required string Location { get; init; }
    [Required]
    public required decimal Latitude { get; init; }
    [Required]
    public required decimal Longitude { get; init; }
    [Required, Length(1, maximumLength: 2)]
    public required Gender[] TargetGenders { get; init; }
    public string Audience => string.Join(", ", TargetGenders);
    [JsonIgnore]
    public string PasswordHash => string.Empty;
    public required string Otp { get; init; }
}