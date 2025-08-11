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
    [Required, MinLength(8)]
    public required string Password { get; init; }
    [JsonIgnore]
    public string PasswordHash => string.Empty;
}