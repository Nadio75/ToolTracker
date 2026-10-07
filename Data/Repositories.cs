using ToolTracker.Models;

namespace ToolTracker.Data;

public interface IRepository<T> where T : Entity
{
    IReadOnlyList<T> GetAll();
    T? GetById(Guid id);
    void Add(T entity);
    void Update(T entity);
    bool Delete(Guid id);
}

public class InMemoryRepository<T> : IRepository<T>
    where T : Entity
{
    private readonly Dictionary<Guid, T> _items = new();

    public IReadOnlyList<T> GetAll() =>
        _items.Values.ToList();

    public T? GetById(Guid id) =>
        _items.GetValueOrDefault(id);

    public void Add(T entity) =>
        _items[entity.Id] = entity;

    public void Update(T entity) =>
        _items[entity.Id] = entity;

    public bool Delete(Guid id) =>
        _items.Remove(id);
}

public interface ILoanRepository : IRepository<Loan>
{
    Loan? GetActiveForTool(Guid toolId);
}

public sealed class LoanRepository :
    InMemoryRepository<Loan>, ILoanRepository
{
    public Loan? GetActiveForTool(Guid toolId) =>
        GetAll().FirstOrDefault(
            l => l.ToolId == toolId && l.IsActive);
}