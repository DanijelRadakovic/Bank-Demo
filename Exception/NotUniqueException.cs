using System;

namespace Bank.Exception
{
    class NotUniqueException : System.Exception
    {
        public NotUniqueException()
        {

        }
        public NotUniqueException(string message) : base(message)
        {
            
        }

        public NotUniqueException(string message, System.Exception inner) : base(message, inner)
        {

        }
    }
}
