using KanbanApp.Requests.SubtaskRequests;
using KanbanApp.Requests.TaskRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubtaskController : ControllerBase
    {
        readonly SubtaskService _subtaskService;
        public SubtaskController(SubtaskService subtaskService)
        {
            _subtaskService = subtaskService;
        }

        [HttpPost]
        [Route("")]
        public IResult CreateSubtask([FromBody] CreateSubtaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _subtaskService.CreateSubtask(request);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("")]
        public IResult EditSubtask([FromBody] EditSubtaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _subtaskService.EditSubtask(request);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("")]
        public IResult DeleteSubtask([FromQuery] Guid subtaskId)
        {
            _subtaskService.DeleteSubtask(subtaskId);
            return Results.Ok();
        }
    }
}
