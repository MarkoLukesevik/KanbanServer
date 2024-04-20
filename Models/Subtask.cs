namespace KanbanApp.Models
{
    public class Subtask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
    }
}
