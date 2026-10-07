using Microsoft.AspNetCore.Mvc;
using ToolTracker.Data;
using ToolTracker.Exceptions;
using ToolTracker.Models;
using ToolTracker.Models.DTOS;

namespace ToolTracker.Controllers;

[ApiController]
[Route("api/tools")]
public class ToolsController(
    IRepository<Tool> repo)
    : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<ToolResponse>> GetAll() =>
        Ok(repo.GetAll().Select(ToolResponse.From));

    [HttpGet("{id:guid}")]
    public ActionResult<ToolResponse> GetById(Guid id) =>
        ToolResponse.From(
            repo.GetById(id)
            ?? throw new NotFoundException(
                nameof(Tool),
                id));
}