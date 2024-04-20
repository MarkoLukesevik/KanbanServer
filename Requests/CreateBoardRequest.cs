using KanbanApp.Models;

namespace KanbanApp.Requests
{
    public class CreateBoardRequest
    {
        public string Name { get; set; }
        public Guid KanbanId { get; set; }
        public List<AddColumnRequest> Columns { get; set; }
    }
}
