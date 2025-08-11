using AutoMapper;
using Commit.Data.Models;
namespace Commit.Dto.Responses;

[AutoMap(typeof(ApplicationUser))]
public class SignInResponse : ApplicationUserResponse
{
    public string? AccessToken { get; set; }
}