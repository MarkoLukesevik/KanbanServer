using KanbanApp.Requests.SubtaskRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubtaskController(SubtaskService subtaskService) : ControllerBase
    {
        [HttpPost]
        [Route("")]
        public async Task<IResult> CreateSubtask([FromBody] CreateSubtaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = await subtaskService.CreateSubtask(request);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("")]
        public async Task<IResult> EditSubtask([FromBody] EditSubtaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = await subtaskService.EditSubtask(request);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IResult> DeleteSubtask([FromQuery] Guid subtaskId)
        {
            await subtaskService.DeleteSubtask(subtaskId);
            return Results.Ok();
        }
    }
}
