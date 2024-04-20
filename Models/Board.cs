namespace KanbanApp.Models
{
    public class Board
    {
        public Board(string name, Guid kanbanId, List<Column> columns) 
        {
            Name = name;
            KanbanId = kanbanId;
            Columns = columns;
        }
        public Guid Id { get; set; }
        public Guid KanbanId { get; set; }
        public string Name { get; set; }
        public List<Column> Columns { get; set; }
    }
}
