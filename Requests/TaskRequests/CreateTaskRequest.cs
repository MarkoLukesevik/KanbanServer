using KanbanApp.Requests.SubtaskRequests;

namespace KanbanApp.Requests.TaskRequests
{
    public class CreateTaskRequest
    {
        public Guid BoardId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<CreateSubtaskRequest> Subtasks { get; set; }
        public int Order { get; set; }
    }
}
