namespace KanbanApp.Models
{
    public class Task
    {
        public Task(Guid columnId, string title, string description, string status, DateTime createdAt, DateTime lastModifiedAt) {
            ColumnId = columnId;
            Title = title;
            Description = description;
            Status = status;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }
        public Guid Id { get; set; }
        public Guid ColumnId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<Subtask> Subtasks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
