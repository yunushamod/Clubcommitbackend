namespace Commit.Dto.Responses;

public class BaseResponse
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}