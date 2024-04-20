using KanbanApp.Database;
using KanbanApp.Models;
using Microsoft.EntityFrameworkCore;

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
            var kanbans = _kanbanContext.Kanbans.Include(x => x.Boards).ThenInclude(x => x.Columns).ThenInclude(x => x.Tasks).ThenInclude(x => x.Subtasks).ToList();
            return kanbans;
        }
    }
}
