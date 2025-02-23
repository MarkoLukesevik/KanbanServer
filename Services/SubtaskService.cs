using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests.SubtaskRequests;

namespace KanbanApp.Services
{
    public class SubtaskService
    {
        readonly KanbanContext _kanbanContext;
        public SubtaskService(KanbanContext kanbanContext)
        {
            _kanbanContext = kanbanContext;
        }

        public Subtask CreateSubtask(CreateSubtaskRequest request)
        {
            if (request.Title.Length <= 0)
                throw new Exceptions.ArgumentException("Subtask title cannot be empty");

            var subtask = new Subtask(request.TaskId, request.Title, false, DateTime.Now, DateTime.Now);
            _kanbanContext.Subtasks.Add(subtask);
            _kanbanContext.SaveChanges();
            return subtask;
        }

        public Subtask EditSubtask(EditSubtaskRequest request)
        {
            var subtask = _kanbanContext.Subtasks.FirstOrDefault(x => x.Id == request.Id);

            if (subtask == null)
                throw new NotFoundException("Subtask with given id was not found.");

            if (request.Title.Length > 0) subtask.Title = request.Title;
            subtask.IsComplete = request.IsComplete;
            subtask.LastModifiedAt = DateTime.Now;
        
            _kanbanContext.SaveChanges();
            return subtask;
        }

        public void DeleteSubtask(Guid subtaskId)
        {
            var subtask = _kanbanContext.Subtasks.FirstOrDefault(x => x.Id == subtaskId);

            if (subtask == null)
                throw new NotFoundException("Subtask with given id was not found.");

            _kanbanContext.Remove(subtask);
            _kanbanContext.SaveChanges();
        }
    }
}
