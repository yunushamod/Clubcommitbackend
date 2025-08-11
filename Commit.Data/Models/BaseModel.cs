namespace Commit.Data.Models;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
}