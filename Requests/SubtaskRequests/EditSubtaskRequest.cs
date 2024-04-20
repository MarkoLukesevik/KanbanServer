namespace KanbanApp.Requests.SubtaskRequests
{
    public class EditSubtaskRequest
    {
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
