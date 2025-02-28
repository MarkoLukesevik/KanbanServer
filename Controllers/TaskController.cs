using KanbanApp.Requests.TaskRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController(TaskService taskService) : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IResult> GetTaskById([FromQuery] Guid taskId)
        {
            var result = await taskService.GetTask(taskId);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IResult> DeleteTaskById([FromQuery] Guid taskId)
        {
            await taskService.DeleteTask(taskId);
            return Results.Ok();
        }

        [HttpPost]
        [Route("")]
        public async Task<IResult> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = await taskService.CreateTask(request);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("")]
        public async Task<IResult> EditTask([FromBody] EditTaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = await taskService.EditTask(request);
            return Results.Ok(result);
        }
    }
}
