using System.Text.Json.Serialization;
namespace Commit.Dto.Responses;

public class Result<T>
{
    public required string Message { get; init; }
    public T? Data { get; init; }
    public required bool Status { get; init; }
    [JsonIgnore]
    public int StatusCode { get; init; }

    public static Result<T> Ok(T data, string message = "Operation completed successfully")
        => new()
        {
            Message = message,
            Data = data,
            Status = true,
            StatusCode = 200,
        };

    public static Result<T> Failed(string message = "An error occured. Please try again", T? data = default)
    => new()
    {
        Message = message,
        Data = data,
        Status = false,
        StatusCode = 400,
    };
    
    public static Result<T> Unauthorized(string message = "Unauthorized request", T? data = default)
        => new()
        {
            Message = message,
            Data = data,
            Status = false,
            StatusCode = 401,
        };
    
    public static Result<T> Forbidden(string message = "You do not have the permission to view this resource", T? data = default)
        => new()
        {
            Message = message,
            Data = data,
            Status = false,
            StatusCode = 403,
        };

    public static Result<T> NotFound(string message = "Resource not found", T? data = default)
        => new()
        {
            Message = message,
            Data = data,
            Status = false,
            StatusCode = 404,
        };

    public static Result<T> InternalError(string message = "An error occured. Please try agan later", T? data = default)
        => new()
        {
            Message = message,
            Data = data,
            Status = false,
            StatusCode = 500,
        };

    public static Result<T> InternalError(Exception ex, T? data = default)
        => new()
        {
            Message = ex.Message,
            Data = data,
            Status = false,
            StatusCode = 500,
        };
}