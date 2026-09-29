namespace ToolTracker.Exceptions;

public abstract class ToolTrackerException : Exception
{
    protected ToolTrackerException(string message) : base(message){}
}