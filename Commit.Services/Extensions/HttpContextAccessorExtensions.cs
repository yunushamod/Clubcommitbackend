using System.Security.Claims;
using Microsoft.AspNetCore.Http;
namespace Commit.Services.Extensions;

public static class HttpContextAccessorExtensions
{
    public static Guid GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        return Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.PrimarySid) ??
                          throw new ArgumentException());
    }
    
    
    // public static UserType GetUserType(this IHttpContextAccessor httpContextAccessor)
    // {
    //     return Enum.Parse<UserType>(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.GivenName) ??
    //                                 throw new ArgumentException());
    // }

    public static string GetEmail(this IHttpContextAccessor httpContextAccessor)
    {
        return httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name) ??
               throw new ArgumentException();
    }
}