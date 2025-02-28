using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KanbanController(KanbanService kanbanService) : ControllerBase
    {
        [HttpGet]
        [Route("")]

        public async Task<IResult> GetAllKanbans([FromQuery] Guid userId)
        {
            var result = await kanbanService.GetKanban(userId);
            return Results.Ok(result);
        }
    }
}
