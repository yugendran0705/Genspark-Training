using System;
using SecureFileAccess.Interfaces;

namespace SecureFileAccess.Roles
{
    public class AdminRole : IRole
    {
        public void ReadFile(IFile realFile)
        {
            realFile.Read();
        }
    }
}
