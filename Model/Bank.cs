using System.Collections.Generic;

namespace Bank.model
{
    class Bank
    {
        public long Id { get; set; }
        public string Name { get; set; }
        internal Account Account { get; set; }
        internal List<Client> Clients { get; set; }
        internal List<Transaction> Transactions { get; set; }
        internal List<Loan> Loans { get; set; }

        public Bank(long id, string name, Account account, List<Client> clients, List<Transaction> transactions, List<Loan> loans)
        {
            Id = id;
            Name = name;
            Account = account;
            Clients = clients;
            Transactions = transactions;
            Loans = loans;
        }
    }
}
