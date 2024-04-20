namespace KanbanApp.Models
{
    public class Column
    {
        public Column(string name) { Name = name; Tasks = new List<Task>(); }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
