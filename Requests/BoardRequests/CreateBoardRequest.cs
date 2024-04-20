using KanbanApp.Requests.ColumnRequests;

namespace KanbanApp.Requests.BoardRequests
{
    public class CreateBoardRequest
    {
        public string Name { get; set; }
        public Guid KanbanId { get; set; }
        public List<AddColumnRequest> Columns { get; set; }
    }
}
