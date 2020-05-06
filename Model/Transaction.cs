using System;

namespace Bank.Model
{
    class Transaction
    {
        public long Id { get; set; }
        public string Purpose { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        internal Client Payer { get; set; }
        internal Client Receiver { get; set; }

        public Transaction(string purpose, double amount, Client payer, Client receiver)
        {
            Purpose = purpose;
            Amount = amount;
            Payer = payer;
            Receiver = receiver;
        }

        public Transaction(long id, string purpose, DateTime date, double amount, Client payer, Client receiver)
        {
            Id = id;
            Purpose = purpose;
            Date = date;
            Amount = amount;
            Payer = payer;
            Receiver = receiver;
        }
    }
}
