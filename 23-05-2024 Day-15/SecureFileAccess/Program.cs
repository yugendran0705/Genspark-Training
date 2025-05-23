using System;
using SecureFileAccess.Models;
using SecureFileAccess.Interfaces;
using SecureFileAccess.Proxies;
using SecureFileAccess.Roles;

namespace SecureFileAccess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine() ?? "Unknown";

            Console.Write("Enter role (Admin, User, Guest): ");
            string roleInput = Console.ReadLine() ?? "Guest";

            IRole role;
            if (roleInput.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                role = new AdminRole();
            }
            else if (roleInput.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                role = new UserRole();
            }
            else
            {
                role = new GuestRole();
            }

            User user = new User(username, role);

            // Use the proxy file to enforce access control.
            IFile secureFile = new ProxyFile(user);
            secureFile.Read();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
