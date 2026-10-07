using FluentValidation;
using ToolTracker.Models.DTOS;

namespace ToolTracker.Validation;

public class CreateLoanRequestValidator
    : AbstractValidator<CreateLoanRequest>
{
    public CreateLoanRequestValidator(TimeProvider clock)
    {
        RuleFor(x => x.ToolId)
            .NotEmpty()
            .WithMessage("ToolId is required.");

        RuleFor(x => x.BorrowerId)
            .NotEmpty()
            .WithMessage("BorrowerId is required.");

        RuleFor(x => x.DueOn)
            .Must(d =>
                d >= DateOnly.FromDateTime(
                    clock.GetUtcNow().UtcDateTime))
            .WithMessage(
                "Due date cannot be in the past.");
    }
}