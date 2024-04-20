using KanbanApp.Requests.ColumnRequests;

namespace KanbanApp.Requests.BoardRequests
{
    public class EditBoardRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<EditColumnRequest> Columns { get; set; }
    }
}
