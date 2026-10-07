using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ToolTracker.Models.DTOS;

namespace ToolTracker.Controllers;

[ApiController]
[Route("api/loans")]
public class LoansController(
    LoanService service,
    IValidator<CreateLoanRequest> validator)
    : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<LoanResponse>> GetAll() =>
        Ok(service.GetAll());

    [HttpGet("{id:guid}")]
    public ActionResult<LoanResponse> GetById(Guid id) =>
        Ok(service.GetById(id));

    /// <summary>
    /// Checks a tool out to a member.
    /// </summary>
    /// <response code="201">
    /// Loan created or original result replayed.
    /// </response>
    /// <response code="400">
    /// Request failed validation.
    /// </response>
    /// <response code="404">
    /// Tool or borrower does not exist.
    /// </response>
    /// <response code="409">
    /// Tool is already checked out.
    /// </response>
    /// <response code="422">
    /// Idempotency-Key was reused with a different payload.
    /// </response>
    [HttpPost]
    [ProducesResponseType(
        typeof(LoanResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<LoanResponse>> Checkout(
        [FromBody] CreateLoanRequest request,
        [FromHeader(Name = "Idempotency-Key")]
        string? idempotencyKey,
        CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(
            request,
            ct);

        var loan =
            service.Checkout(
                request,
                idempotencyKey);

        return CreatedAtAction(
            nameof(GetById),
            new { id = loan.Id },
            loan);
    }

    [HttpPost("{id:guid}/return")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        typeof(ProblemDetails),
        StatusCodes.Status409Conflict)]
    public IActionResult Return(Guid id)
    {
        service.Return(id);

        return NoContent();
    }
}