using SecureFileAccess.Interfaces;

namespace SecureFileAccess.Models
{
    public class User
    {
        public string Username { get; set; }
        public IRole Role { get; set; }

        public User(string username, IRole role)
        {
            Username = username;
            Role = role;
        }
    }
}
