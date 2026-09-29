namespace ToolTracker.Models;

public class Loan
{
    public Guid Id { get; private set;}
    public Guid ToolId {get; private set;}
    public Guid MemberId{get; private set;}
    public decimal Amount{ get; private set;}
    public DateTime CheckOutDate {get;}
    public DateTime DueDate{get; private set;}

    public Loan(Guid toolId, Guid memberId, decimal amount)
    {
        if(toolId == Guid.Empty)
        {
            throw new ArgumentException("A loan must reference a valid Tool.", nameof(toolId));
        }
        if(memberId == Guid.Empty)
        {
            throw new ArgumentException("A loan must reference a valid member.", nameof(memberId));
        }
        if(amount <= 0)
        {
            throw new ArgumentException("A loan amount must greater than zero.", nameof(amount));
        }
        Id = Guid.NewGuid();
        ToolId = toolId;
        MemberId = memberId;
        Amount = amount;
        CheckOutDate = DateTime.UtcNow;
        DueDate = CheckOutDate.AddDays(3);
    }
}