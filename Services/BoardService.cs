using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests;
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

        public Board GetBoardById(Guid boardId)
        {
            var board = _kanbanContext.Boards
                .Include(x => x.Columns)
                .ThenInclude(x => x.Tasks)
                .ThenInclude(x => x.Subtasks)
                .FirstOrDefault(x => x.Id == boardId);

            if (board == null)
            {
                throw new NotFoundException("Board with given id was not found.");
            }

            return board;
        }

        public void DeleteBoardById(Guid boardId)
        {
            var board = _kanbanContext.Boards
                .Include(x => x.Columns)
                .ThenInclude(x => x.Tasks)
                .ThenInclude(x => x.Subtasks)
                .FirstOrDefault(x => x.Id == boardId);

            if (board == null)
            {
                throw new NotFoundException("Board with given id was not found.");
            }

            _kanbanContext.Boards.Remove(board);
            _kanbanContext.SaveChanges();
        }

        public Board CreateBoard(CreateBoardRequest request)
        {
            var kanban = _kanbanContext.Kanbans.FirstOrDefault(x => x.Id == request.KanbanId);
            if (kanban == null)
            {
                throw new NotFoundException("Kanban with given kanbanId was not found.");
            }

            var columns = request.Columns.Select(x => new Column(x.Name)).ToList();
            var board = new Board
            {
                KanbanId = request.KanbanId,
                Name = request.Name,
                Columns = columns,
            };

            _kanbanContext.Boards.Add(board);
            _kanbanContext.SaveChanges();

            return board;
        }
    }
}
