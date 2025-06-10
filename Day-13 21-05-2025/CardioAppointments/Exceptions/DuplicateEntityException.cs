using System;

namespace CardioAppointments.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        private readonly string _message = "Duplicate entity found";
        public DuplicateEntityException(string msg)
        {
            _message = msg;
        }
        public override string Message => _message;
    }
}
