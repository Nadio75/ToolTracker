public class ToolService
{
    private readonly IToolStore _toolStore;

    public ToolService(IToolStore toolStore) => _toolStore = toolStore;

   
    public async Task FindToolAsync(Guid toolId)
    {
        var tool = await _toolStore.GetToolByIdAsync(toollId);
        if (tool is null) throw new NotFoundException("Tool not found.");

        try
        {
            member.AddMember(memberId);
        }
        catch (InvalidOperationException ex)
        {
     
            throw new ConflictException(ex.Message);
        }
    }
     public async Task CheckOutToolAsync(Guid toolId)
    {
        var tool = await _toolStore.GetToolByIdAsync(toollId);
        if (tool is null) throw new NotFoundException("Tool is already checkoed out.");

        try
        {
            member.AddMember(memberId);
        }
        catch (InvalidOperationException ex)
        {
     
            throw new ConflictException(ex.Message);
        }
    }
}