namespace Bank.Model
{
    class Account
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public double Balance { get; set; }


        public Account(long id)
        {
            Id = id;
        }

        public Account(long id, string number, double balance)
        {
            Id = id;
            Number = number;
            Balance = balance;
        }
    }
}
