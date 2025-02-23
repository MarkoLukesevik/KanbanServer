using KanbanApp.Requests.BoardRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardsController : ControllerBase
    {
        readonly BoardService _boardService;
        public BoardsController(BoardService boardService)
        {
            _boardService = boardService;
        }

        [HttpGet]
        [Route("{kanbanId}")]
        public async Task<IResult> GetBoards([FromRoute] Guid kanbanId)
        {
            var result = await _boardService.GetAllMinifiedBoards(kanbanId);
            return Results.Ok(result);
        }

        [HttpGet]
        [Route("")]
        public IResult GetBoardById([FromQuery] Guid id)
        {
            var result = _boardService.GetBoardById(id);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("")]
        public IResult DeleteBoardById([FromQuery] Guid id)
        {
            _boardService.DeleteBoardById(id);
            return Results.Ok();
        }

        [HttpPost]
        [Route("")]
        public IResult CreateBoard([FromBody] CreateBoardRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _boardService.CreateBoard(request);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("")]
        public IResult EditBoard([FromBody] EditBoardRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _boardService.EditBoard(request);
            return Results.Ok(result);
        }
    }
}