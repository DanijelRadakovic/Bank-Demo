namespace Bank.Exception
{
    class InvalidDateException : System.Exception
    {
        public InvalidDateException()
        {

        }

        public InvalidDateException(string message) : base(message)
        {

        }

        public InvalidDateException(string message, System.Exception inner) : base(message, inner)
        {

        }
    }
}
