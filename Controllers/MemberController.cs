using Microsoft.AspNetCore.Mvc;
using ToolTracker.Data;
using ToolTracker.Exceptions;
using ToolTracker.Models;
using ToolTracker.Models.DTOS;

namespace ToolTracker.Controllers;

[ApiController]
[Route("api/members")]
public class MembersController(
    IRepository<Member> repo)
    : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<MemberResponse>> GetAll() =>
        Ok(repo.GetAll().Select(MemberResponse.From));

    [HttpGet("{id:guid}")]
    public ActionResult<MemberResponse> GetById(Guid id) =>
        MemberResponse.From(
            repo.GetById(id)
            ?? throw new NotFoundException(
                nameof(Member),
                id));
}