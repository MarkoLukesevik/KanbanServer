using KanbanApp.Requests.SubtaskRequests;

namespace KanbanApp.Requests.TaskRequests
{
    public class EditTaskRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<EditSubtaskRequest> Subtasks { get; set; }
        public int Order { get; set; }
    }
}
