namespace KanbanApp.Models
{
    public class Kanban
    {
        public Kanban(Guid userId) { Id = userId; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Board> Boards { get; set; }
    }
}
