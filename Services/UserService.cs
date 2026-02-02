using System.Net.Mail;
using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests.UserRequests;
using KanbanApp.Responses;
using Microsoft.EntityFrameworkCore;

namespace KanbanApp.Services
{
    public class UserService(KanbanContext kanbanContext)
    {
        public  async Task<UserResponse> RegisterUser(UserRegisterRequest request)
        {
            var existingUser = await kanbanContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName|| x.Email == request.Email);
            if (existingUser != null)
            {
                throw new ConflictException("User with given username or email already exists.");
            }

            ValidateEmail(request.Email);
            ValidatePassword(request.Password);

            var user = new User(
                request.Name,
                request.LastName,
                request.UserName,
                request.Password,
                request.Email);
            kanbanContext.Users.Add(user);
            await kanbanContext.SaveChangesAsync();

            var kanban = new Kanban(user.Id);
            kanbanContext.Kanbans.Add(kanban);
            await kanbanContext.SaveChangesAsync();
            GetDefaultBoardForNewKanban(kanban);
            await kanbanContext.SaveChangesAsync();

            return new UserResponse { Id = user.Id, UserName = user.UserName };
        }

        public async Task<UserResponse> LoginUser(UserLoginRequest request)
        {
            var user = await kanbanContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName && x.Password == request.Password);

            if (user == null)
            {
                throw new NotFoundException("User with given username or password was not found.");
            }

            return new UserResponse { Id = user.Id, UserName = user.UserName };
        }
        
        private void GetDefaultBoardForNewKanban(Kanban kanban)
        {
            var board = new Board(kanban.Id, "Default board", DateTime.UtcNow, DateTime.UtcNow);
            board.Columns = new List<Column>();
            board.Columns.Add(new Column("Todo", DateTime.UtcNow, DateTime.UtcNow));
            board.Columns.Add(new Column("Doing", DateTime.UtcNow, DateTime.UtcNow));
            board.Columns.Add(new Column("Done", DateTime.UtcNow, DateTime.UtcNow));
            
            kanban.Boards.Add(board);
        }

        private static void ValidateEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                throw new InvalidFormatException("Email is not valid");
            }
            try
            {
                var addr = new MailAddress(email);
                if (addr.Address != trimmedEmail)
                    throw new InvalidFormatException("Email is not valid");
            }
            catch
            {
                throw new InvalidFormatException("Email is not valid");
            }
        }

        private static void ValidatePassword(string password)
        {
            if (password == null)
                throw new InvalidFormatException("Password is required");

            if (password.Length < 6)
                throw new InvalidFormatException("Password must have at least 6 characters");

            var passwordContainsDigits = false;
            foreach (char c in password)
            {
                if (c >= '0' && c <= '9') passwordContainsDigits = true;
            }
            if (!passwordContainsDigits)
                throw new InvalidFormatException("Password must have at least 1 digit");
        }
    }
}
