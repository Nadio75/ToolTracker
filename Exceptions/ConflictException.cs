namespace ToolTracker.Exceptions;

public class ConflictException : ToolTrackerException
{
    public ConflictException(string message) : base(message) { }
}