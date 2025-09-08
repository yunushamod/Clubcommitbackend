using System.Text.Json.Serialization;
using AutoMapper;
using Commit.Data.Models;
namespace Commit.Dto.Responses;

[AutoMap(typeof(ApplicationUser))]
public class BaseApplicationUserResponse : BaseResponse
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; init; }
    public string? ProfileImageUrl { get; init; }
}


[AutoMap(typeof(ApplicationUser))]
public class ApplicationUserResponse : BaseApplicationUserResponse
{
    public required string EmailAddress { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? LastPasswordReset { get; set; }
    public DateTime? LastPasswordChange { get; set; }
    public required string Location { get; init; }
    public required decimal Latitude { get; init; }
    public required decimal Longitude { get; init; }
    [JsonIgnore]
    public required string Audience { get; init; }
    public Gender[] TargetGenders => Audience.Split(", ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(Enum.Parse<Gender>).ToArray();
}