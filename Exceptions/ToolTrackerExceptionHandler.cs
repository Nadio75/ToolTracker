namespace ToolTracker.Exceptions;

public class ToolTrackerExceptionHandler : IExceptionHandler
{
    private readonly ILogger<IExceptionHandler> _logger;

    public IExceptionHandler(ILogger<IExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
{
   
    var correlationId = httpContext.TraceIdentifier;

   
    var (statusCode, title) = exception switch
    {
        NotFoundException => (StatusCodes.Status404NotFound, "Not Found"),
        ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
    };
}}