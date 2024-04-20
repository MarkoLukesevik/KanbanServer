using KanbanApp.Database;
using KanbanApp.Responses;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.Services
{
    public class BoardService
    {
        KanbanContext _kanbanContext;

        public BoardService(KanbanContext kanbanContext)
        {
            _kanbanContext = kanbanContext;
        }

        public async Task<List<MinifiedBoardResponse>> GetAllMinifiedBoards(Guid kanbanId)
        {
            return await _kanbanContext.Boards.Where(x => x.KanbanId == kanbanId)
                .Select(y => new MinifiedBoardResponse(y.Id, y.Name)).ToListAsync();
        }
    }
}
