using KanbanApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.Database
{
    public class KanbanContext : DbContext
    {
        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options)
        {
        }

        public DbSet<Kanban> Kanbans { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
