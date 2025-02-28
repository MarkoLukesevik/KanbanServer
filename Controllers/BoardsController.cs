using KanbanApp.Requests.BoardRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BoardsController(BoardService boardService) : ControllerBase
    {
        [HttpGet]
        [Route("{kanbanId}")]
        public async Task<IResult> GetBoards([FromRoute] Guid kanbanId)
        {
            var result = await boardService.GetAllMinifiedBoards(kanbanId);
            return Results.Ok(result);
        }

        [HttpGet]
        [Route("")]
        public async Task<IResult> GetBoardById([FromQuery] Guid id)
        {
            var result = await boardService.GetBoardById(id);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IResult> DeleteBoardById([FromQuery] Guid id)
        {
            await boardService.DeleteBoardById(id);
            return Results.Ok();
        }

        [HttpPost]
        [Route("")]
        public async Task<IResult> CreateBoard([FromBody] CreateBoardRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = await boardService.CreateBoard(request);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("")]
        public async Task<IResult> EditBoard([FromBody] EditBoardRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = await boardService.EditBoard(request);
            return Results.Ok(result);
        }
    }
}