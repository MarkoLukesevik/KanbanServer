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
        [Route("task{taskId}")]
        public IResult GetTaskById([FromRoute] Guid taskId)
        {
            var result = _taskService.GetTask(taskId);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("task")]
        public IResult DeleteTaskById(Guid taskId)
        {
            _taskService.DeleteTask(taskId);
            return Results.Ok();
        }

        [HttpPost]
        [Route("task")]
        public IResult CreateTask([FromBody] CreateTaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _taskService.CreateTask(request);
            return Results.Ok(result);
        }

        [HttpPut]
        [Route("task")]
        public IResult EditTask([FromBody] EditTaskRequest request)
        {
            if (request == null)
                throw new ArgumentException("request body cannot be empty");

            var result = _taskService.EditTask(request);
            return Results.Ok(result);
        }
    }
}
