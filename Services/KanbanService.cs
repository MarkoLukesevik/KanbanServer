using KanbanApp.Database;
using KanbanApp.Models;

namespace KanbanApp.Services
{
    public class KanbanService
    {
        KanbanContext _kanbanContext;

        public KanbanService(KanbanContext kanbanContext)
        {
            _kanbanContext = kanbanContext;
        }

        public List<Kanban> GetAllKanbans()
        {
            var kanbans = _kanbanContext.Kanbans.Select(x => x).ToList();
            return kanbans;
        }
    }
}
