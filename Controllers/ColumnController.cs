using KanbanApp.Requests.ColumnRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers;

[ApiController]
[Route("[controller]")]
public class ColumnController(ColumnService columnService) : ControllerBase
{
    [HttpPost]
    [Route("")]
    public async Task<IResult> AddColumn([FromBody] AddColumnRequest request)
    {
        if (request == null)
            throw new ArgumentException("request body cannot be empty");

        var result = await columnService.AddColumn(request);
        return Results.Ok(result);
    }
}