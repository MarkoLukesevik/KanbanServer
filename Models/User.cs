namespace KanbanApp.Models
{
    public class User
    {
        public User(string name, string lastName, string userName, string password, string email) {
            Name = name;
            LastName = lastName;
            UserName = userName;
            Password = password;
            Email = email;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
