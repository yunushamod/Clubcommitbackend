using Commit.Dto.Requests;
namespace Commit.Services.Extensions;

public static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationRequest request)
        => queryable.Paginate(request.PageNumber, request.PageSize);

    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int pageNumber, int pageSize)
    {
        if(pageNumber <= 0) pageNumber = 1;
        if(pageSize <= 0) pageSize = 15;
        return queryable.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
    }
}