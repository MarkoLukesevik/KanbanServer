using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests.SubtaskRequests;
using KanbanApp.Requests.TaskRequests;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace KanbanApp.Services
{
    public class TaskService(KanbanContext kanbanContext, SubtaskService subtaskService)
    {
        public async Task<Models.Task> GetTask(Guid taskId)
        {
            var task = await kanbanContext.Tasks
                .Include(x => x.Subtasks)
                .FirstOrDefaultAsync(x => x.Id == taskId);

            if (task == null)
                throw new NotFoundException("Task with given id was not found.");

            return task;
        }

        public async Task DeleteTask(Guid taskId)
        {
            var task = await kanbanContext.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);

            if (task == null)
                throw new NotFoundException("Task with given id was not found.");

            kanbanContext.Tasks.Remove(task);
            await kanbanContext.SaveChangesAsync();
        }

        public async Task<Models.Task> CreateTask(CreateTaskRequest request)
        {
            var board = await kanbanContext.Boards
                .Include(x => x.Columns)
                .FirstOrDefaultAsync(x => x.Id == request.BoardId);
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
            task.Order = request.Order;

            task.Subtasks = new List<Subtask>();
            foreach (var subtask in request.Subtasks)
            {
                var newSubtask = new Subtask(
                    subtask.Title,
                    false,
                    DateTime.Now,
                    DateTime.Now
                );

                task.Subtasks.Add(newSubtask);
            }

            kanbanContext.Tasks.Add(task);
            await kanbanContext.SaveChangesAsync();

            return task;
        }

        public async Task<Models.Task> EditTask(EditTaskRequest request)
        {
            var task = await kanbanContext.Tasks
                .Include(x => x.Subtasks)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (task == null)
                throw new NotFoundException("Task with given id was not found.");

            if (request.Title.Length > 0) task.Title = request.Title;
            if (request.Description.Length > 0) task.Description = request.Description;

            await EditTaskStatus(task, request.Status);
            await EditSubtasks(task, request);
            task.Order = request.Order;
            var tasksInColumn = await kanbanContext.Tasks
                .Where(t => t.ColumnId == task.ColumnId && t.Id != task.Id)
                .OrderBy(t => t.Order)
                .ToListAsync();

            for (int i = 0; i < tasksInColumn.Count; i++)
            {
                if (i >= task.Order)
                    tasksInColumn[i].Order = i + 1; // shift down to make room
                else
                    tasksInColumn[i].Order = i;
            }
            
            task.LastModifiedAt = DateTime.Now;

            await kanbanContext.SaveChangesAsync();

            task.Subtasks = task.Subtasks.OrderBy(x => x.LastModifiedAt).ToList();
            return task;
        }

        private async Task EditTaskStatus(Models.Task task, string status)
        {
            var column = await kanbanContext.Columns.Include(x => x.Tasks)
                .FirstOrDefaultAsync(x => x.Id == task.ColumnId);
            if (column != null)
            {
                var board = await kanbanContext.Boards.Include(x => x.Columns)
                    .FirstOrDefaultAsync(x => x.Id == column.BoardId);
                if (board != null)
                {
                    var columnNames = new List<string>();
                    foreach (var c in board.Columns)
                    {
                        columnNames.Add(c.Name);
                    }

                    if (!columnNames.Contains(status))
                        throw new NotFoundException("There is no column name with the provided status");

                    var newStatusColumn = board.Columns.FirstOrDefault(x => x.Name == status);
                    if (newStatusColumn != null)
                    {
                        if (task.Status != status)
                        {
                            task.Status = status;
                            task.ColumnId = newStatusColumn.Id;
                            column.Tasks.Remove(task);
                            newStatusColumn.Tasks.Add(task);
                        }
                    }
                }
            }
        }

        private async Task EditSubtasks(Models.Task task, EditTaskRequest request)
        {
            var subtasksToRemove = new List<Subtask>();
            foreach (var subtask in task.Subtasks)
            {
                var currentSubtask = request.Subtasks.FirstOrDefault(x => x.Id == subtask.Id);
                if (currentSubtask != null)
                    await subtaskService.EditSubtask(currentSubtask);
                else if (currentSubtask == null)
                    subtasksToRemove.Add(subtask);
            }

            foreach (var subtask in request.Subtasks)
            {
                if (task.Subtasks.Any(x => x.Id == subtask.Id)) continue;
                await subtaskService.CreateSubtask(new CreateSubtaskRequest
                    { Title = subtask.Title, TaskId = request.Id });
            }

            foreach (var subtask in subtasksToRemove)
            {
                kanbanContext.Remove(subtask);
            }
        }
    }
}