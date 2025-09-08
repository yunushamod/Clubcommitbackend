using AutoMapper;
using Commit.Data;
using Commit.Dto.Requests;
using Commit.Dto.Responses;
using Commit.Services.Extensions;
using Commit.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Commit.Services.Implementations;

public class RecommendationService(ApplicationDbContext context,
    IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : IRecommendationService
{
    public async  Task<Result<PaginationResponse<BaseApplicationUserResponse>>> GetRecommendations(PaginationRequest request, 
        CancellationToken cancellationToken = default)
    {
        var user = await context.ApplicationUsers.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == httpContextAccessor.GetUserId(), cancellationToken);
        if(user is null) return Result<PaginationResponse<BaseApplicationUserResponse>>.Unauthorized();
        var offset = (request.PageNumber - 1) * request.PageSize;

        var maxDistance = configuration.GetValue<double>("MaxDistance");
        
        const string countSql = """
                                    SELECT COUNT(*)
                                    FROM ApplicationUsers au
                                    WHERE au.Id <> {0}
                                      AND dbo.CalculateDistanceKm({1}, {2}, au.Latitude, au.Longitude) <= {3}
                                      AND au.Gender IN (SELECT value FROM STRING_SPLIT({4}, ', '))
                                """;
        var totalCount = await context.Database.SqlQueryRaw<int>(
                countSql,
                user.Id,
                user.Latitude,
                user.Longitude,
                maxDistance,
                user.Audience
        ).SingleAsync(cancellationToken);
        
        
        const string sql = """
                           
                                   SELECT au.*
                                   FROM ApplicationUsers au
                                   WHERE au.Id <> {0}
                                     AND dbo.CalculateDistanceKm({1}, {2}, au.Latitude, au.Longitude) <= {3}
                                     AND au.Gender IN (SELECT value FROM STRING_SPLIT({4}, ', '))
                                   ORDER BY dbo.CalculateDistanceKm({1}, {2}, au.Latitude, au.Longitude)
                                   OFFSET {5} ROWS FETCH NEXT {6} ROWS ONLY;
                               
                           """;
        
        
        
        var result =  await context.ApplicationUsers
            .FromSqlRaw(sql,
            user.Id,
            user.Latitude,
            user.Longitude,
            maxDistance,
            user.Audience,
            offset,
            request.PageSize)
            .Select(x => mapper.Map<BaseApplicationUserResponse>(x))
            .ToListAsync(cancellationToken);
        return Result<PaginationResponse<BaseApplicationUserResponse>>.Ok(new(result, totalCount, request.PageNumber));
    }
}