using SecureFileAccess.Interfaces;
using SecureFileAccess.Models;
using SecureFileAccess.RealSubjects;

namespace SecureFileAccess.Proxies
{
    public class ProxyFile : IFile
    {
        private readonly IFile _realFile;
        private readonly User _user;

        public ProxyFile(User user)
        {
            _user = user;
            _realFile = new SecureFileAccess.RealSubjects.File();
        }

        public void Read()
        {
            _user.Role.ReadFile(_realFile);
        }
    }
}
