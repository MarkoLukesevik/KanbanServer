using KanbanApp.Requests.TaskRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        readonly TaskService _taskService;
        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("")]
        public IResult GetTaskById([FromQuery] Guid taskId)
        {
            var result = _taskService.GetTask(taskId);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("")]
        public IResult DeleteTaskById([FromQuery] Guid taskId)
        {
            _taskService.DeleteTask(taskId);
            return Results.Ok();
        }

        [HttpPost]
        [Route("")]
        public IResult CreateTask([FromBody] CreateTaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _taskService.CreateTask(request);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("")]
        public IResult EditTask([FromBody] EditTaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _taskService.EditTask(request);
            return Results.Ok(result);
        }
    }
}
