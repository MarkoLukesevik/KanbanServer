namespace KanbanApp.Models
{
    public class Subtask
    {
        public Subtask(string title, bool isComplete, DateTime createdAt, DateTime lastModifiedAt) {
            Title = title;
            IsComplete = isComplete;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
