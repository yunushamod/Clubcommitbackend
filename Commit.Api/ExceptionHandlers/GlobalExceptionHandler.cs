using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
namespace Commit.Api.ExceptionHandlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        logger.LogError(exception, "Could not process request on Machine {MachineName}. TraceId: {TraceId}", 
        Environment.MachineName, traceId);
        return await Task.FromResult(true);
    }
}