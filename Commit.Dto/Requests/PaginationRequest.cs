namespace Commit.Dto.Requests;

public class PaginationRequest
{
    public int PageSize { get; set; } = 20;
    public int PageNumber { get; set; } = 1;
    public string? SearchKeyword { get; set; }
}