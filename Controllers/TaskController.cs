using KanbanApp.Requests.BoardRequests;
using KanbanApp.Requests.TaskRequests;
using KanbanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace KanbanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        TaskService _taskService;

        public TaskController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        [Route("getTask{taskId}")]
        public IResult GetTaskById([FromRoute] Guid taskId)
        {
            var result = _taskService.GetTask(taskId);
            return Results.Ok(result);
        }

        [HttpDelete]
        [Route("deleteTask")]
        public IResult DeleteTaskById(Guid taskId)
        {
            _taskService.DeleteTask(taskId);
            return Results.Ok();
        }

        [HttpPost]
        [Route("createTask")]
        public IResult CreateTask([FromBody] CreateTaskRequest createTaskRequest)
        {
            if (createTaskRequest == null)
            {
                throw new ArgumentException("request body cannot be empty");
            }

            var result = _taskService.CreateTask(createTaskRequest);
            return Results.Ok(result);
        }
    }
}
