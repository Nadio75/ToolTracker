using System.Runtime.Versioning;

namespace ToolTracker.Contollers;

using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/members")]
public class MemberController : ControllerBase
{
    private readonly IToolStore _store;
    public MemberController(IToolStore store) => _store = store;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAll()
    {
        var members = await _store.GetAllMembersAsync();
        return Ok(members.Select(MemberMapper.ToResponse));
    }
    [HttpGet("{Id}")]
    public async Task<ActionResult<MemberResponse>> GetMemberById()
    {
        var member = await _store.GetMemberById(id);
        if(member is null) throw new NotFoundException("Member not found.");
        return Ok(UMemberMapper.ToResponse(member));
    }
}