namespace ToolTracker.Models;

public class Tool
{
    public Guid Id{get;}
    public string Name{get; private set;}
    public string Category{get; private set;}
    public string Owner{get; private set;}
    public decimal LoanAmount{get; private set;}
    private readonly List<Guid>_borrowerIds = new();
    public IReadOnlyCollection<Guid>BorrowerIds => _borrowerIds.AsReadOnly();

    public Tool(string name, string category, string owner, string loanAmount)
    {
        if (string.IsNullOrWhiteSpace(namne))
        {
            throw new Exception("A tool must have a name", nameof(name));
        }
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new Exception("A topol must have a category", nameof(category));
        }
        if (string.IsNullOrWhiteSpace(owner))
        {
            throw new Exception("A tool must have an owner", nameof(owner));
        }
        if (loanAmount <= 0)
        {
            throw new Exception("A tool loan amount must be greater than zero.", nameof(loanAmount));
        }
        Id = Guid.NewGuid();
        Name = name;
        Category = category;
        Owner = owner;
        LoanAmount = loanAmount;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("A tool cannot have a blank name.", nameof(newName));
        }
    }
    public void ChangeCategory(string newCategory)
    {
        if (string.IsNullOrWhiteSpace(newCategory))
        {
            throw new ArgumentException("A tool's category cannot be blank", nameof(newCategory));
        }
    }
    public void AddBorrower(Guid borrowerId)
    {
        if (_borrowerIds.Contains(borrowerId))
        {
            throw new InvalidOperationException("This member has already borrowed this tool.");
        }
        _borrowerIds.Add(borrowerId);
    }
    public void RemoveBorrower(Guid borrowerId)
    {
        if (_borrowerIds.Contains(borrowerId))
        {
            throw new InvalidOperationException("This member did not borrow this tool.");
        }
        _borrowerIds.Remove(borrowerId);
    }
    public void NewLoanAmount(decimal newLoanAmount)
    {
        if (newLoanAmount <= 0)
        {
            throw new InvalidOperationException("The loan cannot be less than or equal to zero.");
        }
        LoanAmount = newLoanAmount;
    }

}