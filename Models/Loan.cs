using ToolTracker.Exceptions;
namespace ToolTracker.Models;

public enum LoanStatus { CheckedOut, Returned }

public sealed class Loan : Entity
{
    public Guid ToolId { get; }
    public Guid BorrowerId { get; }
    public DateOnly CheckedOutOn { get; }
    public DateOnly DueOn { get; }
    public DateOnly? ReturnedOn { get; private set; }
    public LoanStatus Status { get; private set; }

    private Loan(Guid toolId, Guid borrowerId, DateOnly checkedOutOn, DateOnly dueOn) : base(Guid.NewGuid())
    {
        ToolId = toolId;
        BorrowerId = borrowerId;
        CheckedOutOn = checkedOutOn;
        DueOn = dueOn;
        Status = LoanStatus.CheckedOut;
    }

    public static Loan Checkout(Guid toolId, Guid borrowerId, DateOnly dueOn, DateOnly today)
        => new(toolId, borrowerId, today, dueOn);

    public bool IsActive => Status == LoanStatus.CheckedOut;
    public bool IsOverdue(DateOnly today) => IsActive && today > DueOn;

    public void Return(DateOnly today)
    {
        if (!IsActive)
            throw new ConflictException($"Loan '{Id}' has already been returned.");
        Status = LoanStatus.Returned;
        ReturnedOn = today;
    }
}