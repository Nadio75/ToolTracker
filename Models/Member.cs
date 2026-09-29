using System.Security.Cryptography.X509Certificates;

namespace ToolTracker.Nodels;

public class Member{
    public Guid Id{get;}
    public string Name{get; private set;}
    public string ContactNumber{get; private set;}

    public Member(string name, string ContactNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
           throw new Exception("A member must have a name", nameof(name)); 
        }
        if (string.IsNullOrWhiteSpace)
        {
            throw new Exception("A member must have a contact number", nameof(ContactNumber));
        }
        Id = Guid .NewGuid();
        Name = name;
        ContactNumber = ContactNumber;
    }

    public void Rename(string newName){
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new ArgumentException("A member's name cannot be blank", nameof(newName));
        }
    }
    public void UpdateContactNumber(string newContactNumber)
    {
        if (string.IsNullOrWhiteSpace(newContactNumber))
        {
            throw new ArgumentException ("A member's contact number cannot be blank", nameof(newContactNumber));
        }
    }

}