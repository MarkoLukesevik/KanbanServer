using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests.BoardRequests;
using KanbanApp.Requests.ColumnRequests;
using KanbanApp.Responses;
using Microsoft.EntityFrameworkCore;
using ArgumentException = System.ArgumentException;
using Task = System.Threading.Tasks.Task;

namespace KanbanApp.Services
{
    public class BoardService(KanbanContext kanbanContext)
    {
        public async Task<List<MinifiedBoardResponse>> GetAllMinifiedBoards(Guid kanbanId)
        {
            return await kanbanContext.Boards.Where(x => x.KanbanId == kanbanId)
                .Select(y => new MinifiedBoardResponse(y.Id, y.Name)).ToListAsync();
        }

        public async Task<Board> GetBoardById(Guid boardId)
        {
            var board = await kanbanContext.Boards
                .Include(x => x.Columns)
                .ThenInclude(x => x.Tasks)
                .ThenInclude(x => x.Subtasks)
                .FirstOrDefaultAsync(x => x.Id == boardId);

            if (board == null)
                throw new NotFoundException("Board with given id was not found.");

            return board;
        }

        public async Task DeleteBoardById(Guid boardId)
        {
            var board = await kanbanContext.Boards
                .Include(x => x.Columns)
                .ThenInclude(x => x.Tasks)
                .ThenInclude(x => x.Subtasks)
                .FirstOrDefaultAsync(x => x.Id == boardId);
            if (board == null)
                throw new NotFoundException("Board with given id was not found.");

            kanbanContext.Boards.Remove(board);
            await kanbanContext.SaveChangesAsync();
        }

        public async Task<Board> CreateBoard(CreateBoardRequest request)
        {
            var kanban = await kanbanContext.Kanbans.FirstOrDefaultAsync(x => x.Id == request.KanbanId);
            if (kanban == null)
                throw new NotFoundException("Kanban with given kanbanId was not found.");

            var columnNames = new List<string>();
            foreach (var column in request.Columns)
            {
                columnNames.Add(column.Name);
            }
            if (columnNames.Count != columnNames.Distinct().Count())
                throw new ArgumentException("Columns cannot have duplicate names");

            var columns = request.Columns.Select(x => new Column(x.Name, DateTime.Now, DateTime.Now)).ToList();
            var board = new Board(request.KanbanId, request.Name, DateTime.Now, DateTime.Now);
            board.Columns = columns;

            kanbanContext.Boards.Add(board);
            await kanbanContext.SaveChangesAsync();

            return board;
        }

        public async Task<Board> EditBoard(EditBoardRequest request)
        {
            var board = await kanbanContext.Boards
                .Include(x => x.Columns)
                .ThenInclude(x => x.Tasks)
                .ThenInclude(x => x.Subtasks)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (board == null)
                throw new NotFoundException("Board with given id was not found.");

            if (request.Name.Length > 0) board.Name = request.Name;

            RemoveBoardColumns(board, request.Columns);
            EditBoardColumns(board, request.Columns);

            await kanbanContext.SaveChangesAsync();
            board.Columns = board.Columns.OrderBy(x => x.LastModifiedAt).ToList();
            return board;
        }

        private static void EditBoardColumns (Board board, List<EditColumnRequest> columns)
        {
            var columnNames = new List<string>();
            foreach (var column in columns)
            {
                columnNames.Add(column.Name);
            }
            if (columnNames.Count != columnNames.Distinct().Count())
                throw new ArgumentException("Columns cannot have duplicate names");

            foreach (var column in columns)
            {
                var existingColumn = board.Columns.FirstOrDefault(x => x.Id == column.Id);

                if (existingColumn != null)
                {
                    existingColumn.Name = column.Name;
                    existingColumn.LastModifiedAt = DateTime.Now;

                    foreach (var task in existingColumn.Tasks)
                    {
                        task.Status = column.Name;
                    }
                }
                else
                {
                    var newColumn = new Column(column.Name, DateTime.Now, DateTime.Now);
                    board.Columns.Add(newColumn);
                }
            }
        }

        private void RemoveBoardColumns(Board board, List<EditColumnRequest> columns)
        {
            var columnsToRemove = new List<Column>();
            foreach (var boardColumn in board.Columns)
            {
                var columnToRemove = columns.FirstOrDefault(x => x.Id == boardColumn.Id);
                if (columnToRemove == null)
                    columnsToRemove.Add(boardColumn);
            }

            foreach (var columnToRemove in columnsToRemove)
            {
                if (columnToRemove != null)
                    kanbanContext.Columns.Remove(columnToRemove);
            }
        }
    }
}
