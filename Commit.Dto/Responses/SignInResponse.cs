using AutoMapper;
using Commit.Data.Models;
namespace Commit.Dto.Responses;

[AutoMap(typeof(ApplicationUser))]
public class SignInResponse
{
    public required ApplicationUserResponse User { get; init; }
    public required string AccessToken { get; set; }
}