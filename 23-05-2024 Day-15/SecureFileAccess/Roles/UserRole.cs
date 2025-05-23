using System;
using SecureFileAccess.Interfaces;

namespace SecureFileAccess.Roles
{
    public class UserRole : IRole
    {
        public void ReadFile(IFile realFile)
        {
            Console.WriteLine("[Limited Access] Only file metadata is available. Contact admin for full access.");
        }
    }
}
