using Commit.Dto.Requests;
using Commit.Dto.Responses;
namespace Commit.Services.Interfaces;

public interface IRecommendationService
{
    Task<Result<PaginationResponse<BaseApplicationUserResponse>>> GetRecommendations(PaginationRequest request,
        CancellationToken cancellationToken = default);
}