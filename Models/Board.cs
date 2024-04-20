namespace KanbanApp.Models
{
    public class Board
    {
        public Guid Id { get; set; }
        public Guid KanbanId { get; set; }
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
    }
}
