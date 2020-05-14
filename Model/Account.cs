using Bank.Model.Util;

namespace Bank.Model
{
    public class Account
    {
        public long Id { get; set; }
        public AccountNumber Number { get; set; }
        public double Balance { get; set; }


        public Account(long id)
        {
            Id = id;
        }

        public Account(long id, AccountNumber number, double balance)
        {
            Id = id;
            Number = number;
            Balance = balance;
        }
    }
}
