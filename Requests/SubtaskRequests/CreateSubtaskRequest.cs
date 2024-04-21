namespace KanbanApp.Requests.SubtaskRequests
{
    public class CreateSubtaskRequest
    {
        public string Title { get; set; }
        public Guid TaskId { get; set; }
    }
}
