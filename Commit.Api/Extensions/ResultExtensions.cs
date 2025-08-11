using Commit.Dto.Responses;
using Microsoft.AspNetCore.Mvc;
namespace Commit.Api.Extensions;

public static class ResultExtensions
{
    public static ActionResult<Result<T>> ToActionResult<T>(this Result<T> result)
    {
        return new ObjectResult(result)
        {
            StatusCode = result.StatusCode
        };
    }
}