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
            var board = _kanbanContext.Boards.FirstOrDefault(x => x.Id == boardId);

            if (board == null)
            {
                throw new NotFoundException("Board with given id was not found.");
            }

            return board;
        }

        public void DeleteBoardById(Guid boardId)
        {
            var board = _kanbanContext.Boards.FirstOrDefault(x => x.Id == boardId);

            if (board == null)
            {
                throw new NotFoundException("Board with given id was not found.");
            }

            _kanbanContext.Boards.Remove(board);
            _kanbanContext.SaveChanges();
        }

        public Board CreateBoard(CreateBoardRequest request)
        {
            var boardColumns = request.Columns.Select(x => new Column(x.Name)).ToList();
            var board = new Board(request.Name, request.KanbanId, boardColumns);

            _kanbanContext.Boards.Add(board);
            _kanbanContext.SaveChanges();

            return board;
        }
    }
}
