namespace KanbanApp.Requests.ColumnRequests
{
    public class AddColumnRequest
    {
        public Guid BoardId { get; set; }
        public string Name { get; set; }
    }
}
