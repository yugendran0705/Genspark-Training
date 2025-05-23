using System;
using SecureFileAccess.Interfaces;

namespace SecureFileAccess.Roles
{
    public class GuestRole : IRole
    {
        public void ReadFile(IFile realFile)
        {
            Console.WriteLine("[Access Denied] You do not have permission to read this file.");
        }
    }
}
