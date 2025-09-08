using Commit.Api.Extensions;
using Commit.Dto.Requests;
using Commit.Dto.Responses;
using Commit.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Commit.Api.Controllers;

public class RecommendationController(IRecommendationService recommendationService) : BaseController
{
    [HttpGet, Authorize]
    public async Task<ActionResult<Result<PaginationResponse<BaseApplicationUserResponse>>>> GetRecommendations(
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await recommendationService.GetRecommendations(request, cancellationToken);
        return result.ToActionResult();
    }
}