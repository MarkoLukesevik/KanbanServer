namespace KanbanApp.Models
{
    public class Column
    {
        public Column(string name, DateTime createdAt, DateTime lastModifiedAt) {
            Name = name;
            Tasks = new List<Task>();
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }
        public Guid Id { get; set; }
        public Guid BoardId { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set;}
    }
}
