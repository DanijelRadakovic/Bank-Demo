namespace Bank.Exception
{
    class InvalidLoanDeadline : System.Exception
    {
        public InvalidLoanDeadline()
        {

        }

        public InvalidLoanDeadline(string message) : base(message)
        {

        }

        public InvalidLoanDeadline(string message, System.Exception inner) : base(message, inner)
        {

        }
    }
}
