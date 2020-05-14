using Bank.Model.Util;
using System;

namespace Bank.Model
{
    public class Transaction
    {
        public long Id { get; set; }
        public string Purpose { get; set; }
        public DateTime Date { get; set; }
        public Amount Amount { get; set; }
        public Client Payer { get; set; }
        public Client Receiver { get; set; }

        public Transaction(string purpose, Amount amount, Client payer, Client receiver)
        {
            Purpose = purpose;
            Amount = amount;
            Payer = payer;
            Receiver = receiver;
        }

        public Transaction(long id, string purpose, DateTime date, Amount amount, Client payer, Client receiver)
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
