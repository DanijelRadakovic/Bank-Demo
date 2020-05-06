using Bank.Exception;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank.Model
{
    class Bank
    {

        private const string CLIENT_FILE = "../../Resources/Data/clients.csv";
        private const string ACCOUNT_FILE = "../../Resources/Data/accounts.csv";
        private const string LOAN_FILE = "../../Resources/Data/loans.csv";
        private const string TRANSACTION_FILE = "../../Resources/Data/transactions.csv";
        private const string CSV_DELIMITER = ",";

        private const string DATETIME_FORMAT = "dd.MM.yyyy.";

        private long _clientNextId;
        private long _accountNextId;
        private long _loanNextId;
        private long _transactionNextId;

        private static Bank _instance = null;
        private List<Client> _clients;
        private List<Account> _accounts;
        private List<Loan> _loans;
        private List<Transaction> _transactions;


        private Bank()
        {
            GetAllData();
            InitializeIds();
        }

        public static Bank GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Bank();
            }
            return _instance;
        }

        public List<Client> Clients
        {
            get { return _clients; }
            set { _clients = value; }
        }

        public List<Loan> Loans
        {
            get { return _loans; }
            set { _loans = value; }
        }

        public List<Transaction> Transactions
        {
            get { return _transactions; }
            set { _transactions = value; }
        }

        public Transaction Create(Transaction transaction)
        {
            transaction.Date = DateTime.Now;
            transaction.Payer = FindClientById(transaction.Payer.Id);
            transaction.Receiver = FindClientById(transaction.Receiver.Id);

            ExecuteTransaction(transaction);
            Transaction newTransaction = Save(transaction);

            _transactions.Add(newTransaction);
            return newTransaction;
        }

        public void Update(Transaction transaction)
        {

        }

        public void Delete(Transaction transaction)
        {

        }

        public Loan Create(Loan loan)
        {
            loan.NumberOfPaidIntallments = 0;
            loan.ApprovalDate = DateTime.Now;

            ApproveLoan(loan);
            Loan newLoan = Save(loan);

            _loans.Add(newLoan);
            return newLoan;
        }

        public void Update(Loan loan)
        {

        }

        public void Delete(Loan loan)
        {

        }

        public Client Create(Client client)
        {
            Client newClient;
            string accountNumber = client.Account.Number;

            try
            {
                CheckIsAccountNumberUnique(accountNumber);
                newClient = Save(client);
                _clients.Add(newClient);
                _accounts.Add(newClient.Account);
                return newClient;
            }
            catch (NotUniqueAccountNumber)
            {
                throw;
            }
        }

        public void Update(Client client)
        {

        }

        public void Delete(Client client)
        {

        }

        private void ExecuteTransaction(Transaction transaction)
        {
            transaction.Payer.Account.Balance -= transaction.Amount;
            transaction.Receiver.Account.Balance += transaction.Amount;
        }

        private void ApproveLoan(Loan loan)
        {
            loan.Client.Account.Balance += loan.Base;
        }

        private void CheckIsAccountNumberUnique(string accountNumber)
        {
            foreach (var client in _clients)
            {
                if (client.Account.Number.Equals(accountNumber))
                    throw new NotUniqueAccountNumber($"Account number {accountNumber} already exists");
            }
        }


        private Transaction Save(Transaction transaction)
        {
            transaction.Id = GenerateTransactionId();
            AppendLineToFile(TRANSACTION_FILE, ConvertEntityToCSVFormat(transaction));
            SaveAccounts();
            return transaction;
        }

        private Loan Save(Loan loan)
        {
            loan.Id = GenerateLoanId();
            AppendLineToFile(LOAN_FILE, ConvertEntityToCSVFormat(loan));
            SaveAccounts();
            return loan;
        }

        private Client Save(Client client)
        {
            client.Account = Save(client.Account);
            client.Id = GenerateClientId();
            AppendLineToFile(CLIENT_FILE, ConvertEntityToCSVFormat(client));
            return client;
        }

        private Account Save(Account account)
        {
            account.Id = GenerateAccountId();
            AppendLineToFile(ACCOUNT_FILE, ConvertEntityToCSVFormat(account));
            return account;
        }

        private void SaveAccounts()
        {
            WriteAllLinesToFile(ACCOUNT_FILE,
                _accounts
                .Select(ConvertEntityToCSVFormat)
                .ToList());
        }

        private long GenerateClientId() => _clientNextId++;

        private long GenerateAccountId() => _accountNextId++;

        private long GenerateTransactionId() => _transactionNextId++;

        private long GenerateLoanId() => _loanNextId++;

        private void GetAllData()
        {
            _accounts = ReadAccountsFromFile();
            _clients = ReadClientsFromFile();
            _transactions = ReadTransactionsFromFile();
            _loans = ReadLoansFromFile();

            BindAccountsWithClients();
            BindClientsWithTransactions();
            BindClientsWithLoans();
        }

        private List<Transaction> ReadTransactionsFromFile()
            => File.ReadAllLines(TRANSACTION_FILE)
                .Select(ConvertCSVFormatToTransaction)
                .ToList();

        private List<Loan> ReadLoansFromFile()
            => File.ReadAllLines(LOAN_FILE)
                .Select(ConvertCSVFormatToLoan)
                .ToList();

        private List<Client> ReadClientsFromFile()
            => File.ReadAllLines(CLIENT_FILE)
                .Select(ConvertCSVFormatToClient)
                .ToList();

        private List<Account> ReadAccountsFromFile()
            => File.ReadAllLines(ACCOUNT_FILE)
                .Select(ConvertCSVFormatToAccount)
                .ToList();


        private void BindAccountsWithClients()
            => _clients.ForEach(client => client.Account = FindAccountById(client.Id));


        private void BindClientsWithTransactions()
            => _transactions.ForEach(transaction =>
            {
                transaction.Payer = FindClientById(transaction.Payer.Id);
                transaction.Receiver = FindClientById(transaction.Receiver.Id);
            });

        private void BindClientsWithLoans()
            => _loans.ForEach(loan => loan.Client = FindClientById(loan.Client.Id));

        private Account FindAccountById(long id)
            => _accounts.Find(account => account.Id == id);

        private Client FindClientById(long id)
            => _clients.Find(client => client.Id == id);

        private void InitializeIds()
        {
            _accountNextId = FindMaxIdAmongAccounts(_accounts);
            _clientNextId = FindMaxIdAmongClients(_clients);
            _transactionNextId = FindMaxIdAmongTransactions(_transactions);
            _loanNextId = FindMaxIdAmongLoans(_loans);
        }

        private long FindMaxIdAmongClients(List<Client> clients)
        {
            if (clients.Count == 0)
                return 0;
            return clients.Max(client => client.Id);
        }

        private long FindMaxIdAmongAccounts(List<Account> accounts)
        {
            if (accounts.Count == 0)
                return 0;
            return accounts.Max(account => account.Id);
        }

        private long FindMaxIdAmongTransactions(List<Transaction> transactions)
        {
            if (transactions.Count == 0)
                return 0;
            return transactions.Max(transaction => transaction.Id);
        }

        private long FindMaxIdAmongLoans(List<Loan> loans)
        {
            if (loans.Count == 0)
                return 0;
            return loans.Max(loan => loan.Id);
        }

        private string ConvertEntityToCSVFormat(Transaction transaction)
            => string.Join(CSV_DELIMITER,
                transaction.Id,
                transaction.Purpose,
                transaction.Date.ToString(DATETIME_FORMAT),
                transaction.Amount,
                transaction.Payer.Id,
                transaction.Receiver.Id);

        private Transaction ConvertCSVFormatToTransaction(string transactionCSVFormat)
        {
            string[] tokens = transactionCSVFormat.Split(CSV_DELIMITER.ToCharArray());
            return new Transaction(
                long.Parse(tokens[0]),
                tokens[1],
                DateTime.Parse(tokens[2]),
                double.Parse(tokens[3]),
                new Client(long.Parse(tokens[4])),
                new Client(long.Parse(tokens[5])));
        }

        private string ConvertEntityToCSVFormat(Loan loan)
            => string.Join(CSV_DELIMITER,
                loan.Id,
                loan.Client.Id,
                loan.ApprovalDate.ToString(DATETIME_FORMAT),
                loan.Deadline.ToString(DATETIME_FORMAT),
                loan.Base,
                loan.InterestRate,
                loan.NumberOfInstallments,
                loan.InstallmentAmount,
                loan.NumberOfPaidIntallments);

        private Loan ConvertCSVFormatToLoan(string loanCSVFormat)
        {
            string[] tokens = loanCSVFormat.Split(CSV_DELIMITER.ToCharArray());
            return new Loan(
                long.Parse(tokens[0]),
                new Client(long.Parse(tokens[1])),
                DateTime.Parse(tokens[2]),
                DateTime.Parse(tokens[3]),
                double.Parse(tokens[4]),
                double.Parse(tokens[5]),
                long.Parse(tokens[6]),
                double.Parse(tokens[7]),
                long.Parse(tokens[8]));
        }

        private string ConvertEntityToCSVFormat(Client client)
            => string.Join(CSV_DELIMITER,
                client.Id,
                client.FirstName,
                client.LastName,
                client.DateOfBirth.ToString(DATETIME_FORMAT),
                client.Account.Id);

        private Client ConvertCSVFormatToClient(string clientCSVFormat)
        {
            string[] tokens = clientCSVFormat.Split(CSV_DELIMITER.ToCharArray());
            return new Client(
                long.Parse(tokens[0]),
                tokens[1], tokens[2],
                DateTime.Parse(tokens[3]),
                new Account(long.Parse(tokens[4])));
        }

        private string ConvertEntityToCSVFormat(Account account)
            => string.Join(CSV_DELIMITER,
                account.Id,
                account.Number,
                account.Balance);

        private Account ConvertCSVFormatToAccount(string acountCSVFormat)
        {
            string[] tokens = acountCSVFormat.Split(CSV_DELIMITER.ToCharArray());
            return new Account(long.Parse(tokens[0]), tokens[1], double.Parse(tokens[2]));
        }

        private void AppendLineToFile(string path, string line)
            => File.AppendAllText(path, line + Environment.NewLine);

        private void WriteAllLinesToFile(string path, List<string> content)
            => File.WriteAllLines(path, content.ToArray());
    }
}
