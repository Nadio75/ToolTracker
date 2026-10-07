using System.Security.Cryptography;
using System.Text.Json;
using ToolTracker.Data;
using ToolTracker.Exceptions;
using ToolTracker.Models;
using ToolTracker.Models.DTOS;

namespace ToolTracker.Services;

public sealed class LoanService(
    ILoanRepository loans,
    IRepository<Tool> tools,
    IRepository<Member> members,
    TimeProvider clock)
{
    private readonly object _gate = new();

    private readonly Dictionary<string, (string Hash, LoanResponse Response)>
        _idempotency = new();

    private DateOnly Today =>
        DateOnly.FromDateTime(clock.GetUtcNow().UtcDateTime);

    public IReadOnlyList<LoanResponse> GetAll() =>
        loans.GetAll()
            .Select(LoanResponse.From)
            .ToList();

    public LoanResponse GetById(Guid id) =>
        LoanResponse.From(
            loans.GetById(id)
            ?? throw new NotFoundException(nameof(Loan), id));

    public LoanResponse Checkout(
        CreateLoanRequest request,
        string? idempotencyKey)
    {
        lock (_gate)
        {
            string? hash = null;

            if (!string.IsNullOrWhiteSpace(idempotencyKey))
            {
                hash = Fingerprint(request);

                if (_idempotency.TryGetValue(
                    idempotencyKey,
                    out var saved))
                {
                    if (saved.Hash != hash)
                    {
                        throw new IdempotencyKeyReuseException(
                            idempotencyKey);
                    }

                    return saved.Response;
                }
            }

            var tool =
                tools.GetById(request.ToolId)
                ?? throw new NotFoundException(
                    nameof(Tool),
                    request.ToolId);

            _ = members.GetById(request.BorrowerId)
                ?? throw new NotFoundException(
                    nameof(Member),
                    request.BorrowerId);

            // Business rule:
            // A tool cannot have more than one active loan.
            if (loans.GetActiveForTool(tool.Id) is not null)
            {
                throw new ConflictException(
                    $"Tool '{tool.Name}' is already checked out.");
            }

            var loan = Loan.Checkout(
                tool.Id,
                request.BorrowerId,
                request.DueOn,
                Today);

            loans.Add(loan);

            var response = LoanResponse.From(loan);

            if (hash is not null)
            {
                _idempotency[idempotencyKey!] =
                    (hash, response);
            }

            return response;
        }
    }

    public void Return(Guid loanId)
    {
        lock (_gate)
        {
            var loan =
                loans.GetById(loanId)
                ?? throw new NotFoundException(
                    nameof(Loan),
                    loanId);

            loan.Return(Today);

            loans.Update(loan);
        }
    }

    private static string Fingerprint(
        CreateLoanRequest request) =>
        Convert.ToHexString(
            SHA256.HashData(
                JsonSerializer.SerializeToUtf8Bytes(request)));
}