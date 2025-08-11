using AutoMapper;
using Commit.Data.Models;
namespace Commit.Dto.Responses;

[AutoMap(typeof(ApplicationUser))]
public class ApplicationUserResponse : BaseResponse
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
    public required string PhoneNumber { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? LastPasswordReset { get; set; }
    public DateTime? LastPasswordChange { get; set; }
}