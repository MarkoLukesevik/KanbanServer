using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.Services
{
    public class KanbanService(KanbanContext kanbanContext)
    {
        public async Task<Kanban> GetKanban(Guid userId)
        {
            var kanban =  await kanbanContext.Kanbans
                .Include(x => x.Boards)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (kanban == null)
                throw new NotFoundException("No Kanban for logged user found.");

            return kanban;
        }
    }
}
