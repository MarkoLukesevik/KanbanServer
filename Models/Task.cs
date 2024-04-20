namespace KanbanApp.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<Subtask> Subtasks { get; set; }
    }
}
