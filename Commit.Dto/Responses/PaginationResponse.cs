namespace Commit.Dto.Responses;

public class PaginationResponse<T>(IEnumerable<T>? data, long count, int pageNumber)
{
    public IEnumerable<T> Data { get; init; } = data ?? [];
    public long Count { get; init; } = count;
    public int PageNumber { get; init; } = pageNumber;
}