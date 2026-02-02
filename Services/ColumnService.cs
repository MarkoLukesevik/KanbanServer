using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Requests.ColumnRequests;
using Microsoft.EntityFrameworkCore;
using Column = KanbanApp.Models.Column;

namespace KanbanApp.Services;

public class ColumnService(KanbanContext kanbanContext)
{
    public async Task<Column> AddColumn(AddColumnRequest request)
    {
        var board = await kanbanContext.Boards
            .Include(x => x.Columns)
            .FirstOrDefaultAsync(x => x.Id == request.BoardId);
        
        if (board == null)
            throw new NotFoundException("Board with given board id was not found.");
        
        if (board.Columns.Any(x => x.Name == request.Name)) 
            throw new NotFoundException("There is already a column with the name provided fot this board.");

        var column = new Column(request.Name, DateTime.UtcNow, DateTime.UtcNow);
        board.Columns.Add(column);
        await kanbanContext.SaveChangesAsync();
        
        return column;
    }
}