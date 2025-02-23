using System.Runtime.InteropServices;
using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.Services
{
    public class KanbanService
    {
        readonly KanbanContext _kanbanContext;
        public KanbanService(KanbanContext kanbanContext)
        {
            _kanbanContext = kanbanContext;
        }

        public async Task<Kanban> GetKanban(Guid userId)
        {
            var kanban =  await _kanbanContext.Kanbans.Include(x => x.Boards).ThenInclude(x => x.Columns).ThenInclude(x => x.Tasks).ThenInclude(x => x.Subtasks).FirstOrDefaultAsync(x => x.UserId == userId);

            if (kanban == null)
            {
                throw new NotFoundException("No Kanban for logged user found.");
            }

            return kanban;
        }
    }
}
