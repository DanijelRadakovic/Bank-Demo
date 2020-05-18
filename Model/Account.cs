using Bank.Model.Util;
using Bank.Repository.Abstract;

namespace Bank.Model
{
    public class Account : IIdentifiable<long>
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

        public long GetId() => Id;

        public void SetId(long id) => Id = id;
    }
}
