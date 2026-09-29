using ToolTracker.Exceptions;

namespace ToolTracker.Exceptions;



public class NotFoundException : ToolTrackerException
{
    public NotFoundException(string message) : base(message) { }
}