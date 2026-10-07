using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ToolTracker.Exceptions;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext ctx,
        Exception ex,
        CancellationToken ct)
    {
        var (status, title, type) = ex switch
        {
            NotFoundException =>
                (404,
                 "Resource not found",
                 "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.5"),

            ConflictException =>
                (409,
                 "Conflict",
                 "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.10"),

            IdempotencyKeyReuseException =>
                (422,
                 "Idempotency key reused",
                 "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.21"),

            ValidationException =>
                (400,
                 "Validation failed",
                 "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.1"),

            _ =>
                (500,
                 "An unexpected error occurred",
                 "https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1")
        };

        if (status == 500)
        {
            logger.LogError(
                ex,
                "Unhandled exception");
        }

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = type,
            Detail = status == 500
                ? "Something went wrong on our side."
                : ex.Message,
            Instance = ctx.Request.Path
        };

        if (ex is ValidationException validationException)
        {
            problem.Extensions["errors"] =
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage)
                              .ToArray());
        }

        ctx.Response.StatusCode = status;

        await ctx.Response.WriteAsJsonAsync(
            problem,
            options: null,
            contentType: "application/problem+json",
            cancellationToken: ct);

        return true;
    }
}