using System;

namespace Bank.Exception
{
    class NotUniqueAccountNumber : System.Exception
    {
        public NotUniqueAccountNumber()
        {

        }
        public NotUniqueAccountNumber(string message) : base(message)
        {
            
        }

        public NotUniqueAccountNumber(string message, System.Exception inner) : base(message, inner)
        {

        }
    }
}
