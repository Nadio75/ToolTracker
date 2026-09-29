namespace ToolTracker.Data;

public class InMemoryToolStore : IToolStore
{
    private readonly List<Member> _members = new();
    private readonly List<Tool> _tools = new();
    public InMemoryToolStore()
    {
        var nadio = new Member("Nadio Maja", "0123456789");
        var tshego = new Member("Tshegofatso Moima", "0987654321");
        var misper = new Member("Misper Madyira", "0567891234");
        _members.Add(nadio);
        _members.Add(tshego);
        _members.Add(misper);

     //   var lawnMower = new Tool("")
    }
}