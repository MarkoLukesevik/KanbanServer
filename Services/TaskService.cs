using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests.TaskRequests;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.Services
{
    public class TaskService
    {
        KanbanContext _kanbanContext;

        public TaskService(KanbanContext kanbanContext)
        {
            _kanbanContext = kanbanContext;
        }

        public Models.Task GetTask(Guid taskId)
        {
            var task = _kanbanContext.Tasks.Include(x => x.Subtasks).FirstOrDefault(x => x.Id == taskId);

            if (task == null)
            {
                throw new NotFoundException("Task with given id was not found.");
            }

            return task;
        }

        public void DeleteTask(Guid taskId)
        {
            var task = _kanbanContext.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (task == null)
            {
                throw new NotFoundException("Task with given id was not found.");
            }

            _kanbanContext.Tasks.Remove(task);
            _kanbanContext.SaveChanges();
        }

        public Models.Task CreateTask(CreateTaskRequest request)
        {
            var board = _kanbanContext.Boards.Include(x => x.Columns).FirstOrDefault(x => x.Id == request.BoardId);
            if (board == null)
            {
                throw new NotFoundException("Board with given boardId was not found.");
            }

            var column = board.Columns.FirstOrDefault(x => x.Name == request.Status);

            if (column == null)
            {
                throw new NotFoundException("Column not found.");
            }

            var task = new Models.Task(
                column.Id,
                request.Title,
                request.Description,
                request.Status,
                DateTime.Now,
                DateTime.Now);

            task.Subtasks = request.Subtasks.Select(x => new Subtask(x.Title, false, DateTime.Now, DateTime.Now)).ToList();

            _kanbanContext.Tasks.Add(task);
            _kanbanContext.SaveChanges();

            return task;
        }
    }
}
