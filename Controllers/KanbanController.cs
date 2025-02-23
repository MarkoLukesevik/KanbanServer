using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KanbanController : ControllerBase
    {
        readonly KanbanService _kanbanService;
        public KanbanController(KanbanService kanbanService)
        {
            _kanbanService = kanbanService;
        }
        [HttpGet]
        [Route("")]

        public IResult GetAllKanbans([FromQuery] Guid userId)
        {
            var result = _kanbanService.GetKanban(userId);
            return Results.Ok(result);
        }
    }
}
