using System.Net.Mail;
using KanbanApp.Database;
using KanbanApp.Exceptions;
using KanbanApp.Models;
using KanbanApp.Requests.UserRequests;
using KanbanApp.Responses;

namespace KanbanApp.Services
{
    public class UserService
    {
        readonly KanbanContext _kanbanContext;
        public UserService(KanbanContext kanbanContext)
        {
            _kanbanContext = kanbanContext;
        }

        public UserResponse RegisterUser(UserRegisterRequest request)
        {
            var existingUser = _kanbanContext.Users.FirstOrDefault(x => x.UserName == request.UserName);
            if (existingUser != null)
            {
                throw new ConflictException("User with given username already exists.");
            }

            ValidateEmail(request.Email);
            ValidatePassword(request.Password);

            var user = new User(
                request.Name,
                request.LastName,
                request.UserName,
                request.Password,
                request.Email);
            _kanbanContext.Users.Add(user);
            _kanbanContext.SaveChanges();

            var kanban = new Kanban(user.Id);
            _kanbanContext.Kanbans.Add(kanban);
            _kanbanContext.SaveChanges();

            return new UserResponse { Id = user.Id, UserName = user.UserName };
        }

        public UserResponse LoginUser(UserLoginRequest request)
        {
            var user = _kanbanContext.Users.FirstOrDefault(x => x.UserName == request.UserName && x.Password == request.Password);

            if (user == null)
            {
                throw new NotFoundException("User with given username or password was not found.");
            }

            return new UserResponse { Id = user.Id, UserName = user.UserName };
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

            if (password.Length <= 6)
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
