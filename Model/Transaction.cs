using System;

namespace Bank.model
{
    class Transaction
    {
        public long Id { get; set; }
        public string Purpose { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        internal Client Payer { get; set; }
        internal Client Receiver { get; set; }
        internal Bank Bank { get; set; }
        public double CommissionFee { get; set; }

        public Transaction(long id, string purpose, DateTime date, double amount,
            Client payer, Client receiver, Bank bank, double commissionFee)
        {
            Id = id;
            Purpose = purpose;
            Date = date;
            Amount = amount;
            Payer = payer;
            Receiver = receiver;
            Bank = bank;
            CommissionFee = commissionFee;
        }
    }
}
