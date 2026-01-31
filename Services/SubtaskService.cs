using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests.SubtaskRequests;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace KanbanApp.Services
{
    public class SubtaskService(KanbanContext kanbanContext)
    {
        public async Task<Subtask> CreateSubtask(CreateSubtaskRequest request)
        {
            if (request.Title.Length <= 0)
                throw new Exceptions.ArgumentException("Subtask title cannot be empty");

            var subtask = new Subtask(
                request.Title,
                false,
                DateTime.Now,
                DateTime.Now
                );
            
            subtask.TaskId = request.TaskId;
            
            kanbanContext.Subtasks.Add(subtask);
            await kanbanContext.SaveChangesAsync();
            return subtask;
        }

        public async Task<Subtask> EditSubtask(EditSubtaskRequest request)
        {
            var subtask = await kanbanContext.Subtasks.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (subtask == null)
                throw new NotFoundException("Subtask with given id was not found.");

            if (request.Title.Length > 0) subtask.Title = request.Title;
            subtask.IsComplete = request.IsComplete;
            subtask.LastModifiedAt = DateTime.Now;
        
            await kanbanContext.SaveChangesAsync();
            return subtask;
        }

        public async Task DeleteSubtask(Guid subtaskId)
        {
            var subtask = await kanbanContext.Subtasks.FirstOrDefaultAsync(x => x.Id == subtaskId);

            if (subtask == null)
                throw new NotFoundException("Subtask with given id was not found.");

            kanbanContext.Remove(subtask);
            await kanbanContext.SaveChangesAsync();
        }
    }
}
