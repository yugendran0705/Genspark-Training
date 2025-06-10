using System;

namespace WholeApplication.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        private string _message = "Duplicate entity found";
        public DuplicateEntityException(string msg)
        {
            _message = msg;
        }
        public override string Message => _message;
    }
}
