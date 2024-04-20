namespace KanbanApp.Models
{
    public class Kanban
    {
        public Kanban(Guid userId) {
            UserId = userId;
            Boards = new List<Board>();
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Board> Boards { get; set; }
    }
}
