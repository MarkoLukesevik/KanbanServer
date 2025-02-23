namespace KanbanApp.Requests.SubtaskRequests
{
    public class EditSubtaskRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
