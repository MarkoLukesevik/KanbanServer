using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        BoardService _boardService;

        public BoardController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet]
        [Route("/getBoards/{kanbanId}")]
        public async Task<IResult> GetBoards([FromRoute] Guid kanbanId)
        {
            var result = await _boardService.GetAllMinifiedBoards(kanbanId);
            return Results.Ok(result);
        }
    }
}
