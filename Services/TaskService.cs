using Azure.Core;
using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests.SubtaskRequests;
using KanbanApp.Requests.TaskRequests;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.Services
{
    public class TaskService
    {
        readonly KanbanContext _kanbanContext;
        readonly SubtaskService _subtaskService;
        public TaskService(KanbanContext kanbanContext, SubtaskService subtaskService)
        {
            _kanbanContext = kanbanContext;
            _subtaskService = subtaskService;
        }

        public Models.Task GetTask(Guid taskId)
        {
            var task = _kanbanContext.Tasks.Include(x => x.Subtasks).FirstOrDefault(x => x.Id == taskId);

            if (task == null)
                throw new NotFoundException("Task with given id was not found.");

            return task;
        }

        public void DeleteTask(Guid taskId)
        {
            var task = _kanbanContext.Tasks.FirstOrDefault(x => x.Id == taskId);

            if (task == null)
                throw new NotFoundException("Task with given id was not found.");

            _kanbanContext.Tasks.Remove(task);
            _kanbanContext.SaveChanges();
        }

        public Models.Task CreateTask(CreateTaskRequest request)
        {
            var board = _kanbanContext.Boards.Include(x => x.Columns).FirstOrDefault(x => x.Id == request.BoardId);
            if (board == null)
                throw new NotFoundException("Board with given boardId was not found.");

            var column = board.Columns.FirstOrDefault(x => x.Name == request.Status);

            if (column == null)
                throw new NotFoundException("Column not found.");

            var task = new Models.Task(
                column.Id,
                request.Title,
                request.Description,
                request.Status,
                DateTime.Now,
                DateTime.Now);

            foreach (var subtask in request.Subtasks)
            {
                task.Subtasks.Add(_subtaskService.CreateSubtask(subtask));
            }

            _kanbanContext.Tasks.Add(task);
            _kanbanContext.SaveChanges();

            return task;
        }

        public Models.Task EditTask(EditTaskRequest request)
        {
            var task = _kanbanContext.Tasks.Include(x => x.Subtasks).FirstOrDefault(x => x.Id == request.Id);
            if (task == null)
                throw new NotFoundException("Task with given id was not found.");

            if (request.Title.Length > 0) task.Title = request.Title;
            if (request.Description.Length > 0) task.Description = request.Description;

            EditTaskStatus(task, request.Status);
            EditSubtasks(task, request);

            task.LastModifiedAt = DateTime.Now;

            _kanbanContext.SaveChanges();

            task.Subtasks = task.Subtasks.OrderBy(x => x.LastModifiedAt).ToList();
            return task;
        }

        private void EditTaskStatus(Models.Task task, string status)
        {
            var column = _kanbanContext.Columns.FirstOrDefault(x => x.Id != task.ColumnId);
            if (column != null)
            {
                var board = _kanbanContext.Boards.FirstOrDefault(x => x.Id != column.BoardId);
                if (board != null)
                {
                    var columnNames = new List<string>();
                    foreach (var c in board.Columns)
                    {
                        columnNames.Add(c.Name);
                    }

                    if (!columnNames.Contains(status))
                        throw new NotFoundException("There is no column name with the provided status");
                    task.Status = status;
                }
            }
        }

        private void EditSubtasks(Models.Task task, EditTaskRequest request)
        {
            var subtasksToRemove = new List<Subtask>();
            foreach (var subtask in task.Subtasks)
            {
                var currentSubtask = request.Subtasks.FirstOrDefault(x => x.Id == subtask.Id);
                if (currentSubtask != null)
                    _subtaskService.EditSubtask(currentSubtask);
                else if (currentSubtask == null)
                    subtasksToRemove.Add(subtask);
            }

            foreach (var subtask in request.Subtasks)
            {
                if (subtask.Id == null)
                    _subtaskService.CreateSubtask(new CreateSubtaskRequest { Title = subtask.Title, TaskId = request.Id });
            }

            foreach (var subtask in subtasksToRemove)
            {
                _kanbanContext.Remove(subtask);
            }
        }
    }
}
