using KanbanApp.Requests.BoardRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardController : ControllerBase
    {
        readonly BoardService _boardService;
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

        [HttpGet]
        [Route("/getBoard/{boardId}")]
        public IResult GetBoardById([FromRoute] Guid boardId)
        {
            var result = _boardService.GetBoardById(boardId);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("/deleteBoard/{boardId}")]
        public IResult DeleteBoardById([FromRoute] Guid boardId)
        {
            _boardService.DeleteBoardById(boardId);
            return Results.Ok();
        }

        [HttpPost]
        [Route("createBoard")]
        public IResult CreateBoard([FromBody] CreateBoardRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _boardService.CreateBoard(request);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("editBoard")]
        public IResult EditBoard([FromBody] EditBoardRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _boardService.EditBoard(request);
            return Results.Ok(result);
        }
    }
}