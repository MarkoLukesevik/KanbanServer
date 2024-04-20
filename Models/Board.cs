namespace KanbanApp.Models
{
    public class Board
    {
        public Board(Guid kanbanId, string name, DateTime createdAt, DateTime lastModifiedAt) {
            KanbanId = kanbanId;
            Name = name;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }
        public Guid Id { get; set; }
        public Guid KanbanId { get; set; }
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
