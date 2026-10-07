namespace ToolTracker.Models.DTOS;

public record CreateLoanRequest(Guid ToolId, Guid BorrowerId, DateOnly DueOn);

public record LoanResponse(Guid Id, Guid ToolId, Guid BorrowerId,
    DateOnly CheckedOutOn, DateOnly DueOn, DateOnly? ReturnedOn, string Status)
{
    public static LoanResponse From(Loan l) =>
        new(l.Id, l.ToolId, l.BorrowerId, l.CheckedOutOn, l.DueOn, l.ReturnedOn, l.Status.ToString());
}

public record ToolResponse(Guid Id, string Name, string Category, Guid OwnerId)
{
    public static ToolResponse From(Tool t) => new(t.Id, t.Name, t.Category, t.OwnerId);
}

public record MemberResponse(Guid Id, string Name, string Email)
{
    public static MemberResponse From(Member m) => new(m.Id, m.Name, m.Email);
}