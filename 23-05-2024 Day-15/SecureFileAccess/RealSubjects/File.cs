using System;
using SecureFileAccess.Interfaces;

namespace SecureFileAccess.RealSubjects
{
    public class File : IFile
    {
        public void Read()
        {
            Console.WriteLine("[Access Granted] Reading sensitive file content...");
        }
    }
}
