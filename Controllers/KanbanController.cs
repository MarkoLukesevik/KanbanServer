using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KanbanController : ControllerBase
    {
        KanbanService _kanbanService;

        public KanbanController(KanbanService kanbanService)
        {
            _kanbanService = kanbanService;
        }
        [HttpGet]
        [Route("getAllKanbans")]

        public IResult GetAllKanbans()
        {
            var result = _kanbanService.GetAllKanbans();
            return Results.Ok(result);
        }
    }
}
