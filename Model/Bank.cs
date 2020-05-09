using Bank.Exception;
using Bank.Model.Util;
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

            SetMissingValues(transaction);
            ExecuteTransaction(transaction);
            Transaction newTransaction = Save(transaction);

            _transactions.Add(newTransaction);
            return newTransaction;
        }

        public void Update(Transaction transaction)
        {
            try
            {
                _transactions[_transactions.FindIndex(tr => tr.Id == transaction.Id)] = transaction;
                SaveTransactions();
            }
            catch (ArgumentNullException)
            {
                throw new EntityNotFoundException($"Transaction with id: {transaction.Id} can not be found!");
            }
        }

        public void Delete(Transaction transaction)
        {
            var transactionToRemove = _transactions.SingleOrDefault(tr => tr.Id == transaction.Id);
            if (transactionToRemove != null)
            {
                _transactions.Remove(transactionToRemove);
                SaveTransactions();
            }
            else
            {
                throw new EntityNotFoundException($"Transaction with id: {transaction.Id} can not be found!");
            }

        }

        public Loan Create(Loan loan)
        {
            SetMissingValues(loan);
            if (IsDeadlineAfterApprovalDate(loan))
            {
                SetNumberOfInstallments(loan);
                SetInstallmentAmount(loan);
                ApproveLoan(loan);
                Loan newLoan = Save(loan);

                _loans.Add(newLoan);
                return newLoan;
            }
            else
            {
                throw new InvalidDateException($"Deadline: {loan.Deadline} is before approval date: {loan.ApprovalDate}");
            }

        }

        public void Update(Loan loan)
        {
            try
            {
                _loans[_loans.FindIndex(ln => ln.Id == loan.Id)] = loan;
                SaveLoans();
            }
            catch (ArgumentNullException)
            {
                throw new EntityNotFoundException($"Loan with id: {loan.Id} can not be found!");
            }

        }

        public void Delete(Loan loan)
        {
            var loanToRemove = _loans.SingleOrDefault(ln => ln.Id == loan.Id);
            if (loanToRemove != null)
            {
                _loans.Remove(loanToRemove);
                SaveLoans();
            }
            else
            {
                throw new EntityNotFoundException($"Loan with id: {loan.Id} can not be found!");
            }
        }

        public Client Create(Client client)
        {
            Client newClient;
            string accountNumber = client.Account.Number.Value;

            if (IsAccountNumberUnique(accountNumber))
            {
                newClient = Save(client);
                _clients.Add(newClient);
                _accounts.Add(newClient.Account);
                return newClient;
            }
            else
            {
                throw new NotUniqueException($"Account number {accountNumber} already exists");
            }
        }

        public void Update(Client client)
        {
            try
            {
                _clients[_clients.FindIndex(clt => clt.Id == client.Id)] = client;
                SaveClients();
            }
            catch (ArgumentNullException)
            {
                throw new EntityNotFoundException($"Client with id: {client.Id} can not be found!");
            }

        }

        public void Delete(Client client)
        {
            var clientToRemove = _clients.SingleOrDefault(cnt => cnt.Id == client.Id);
            if (clientToRemove != null)
            {
                _clients.Remove(clientToRemove);
                SaveClients();
            }
            else
            {
                throw new EntityNotFoundException($"Client with id: {client.Id} can not be found!");
            }
        }

        private void SetMissingValues(Transaction transaction)
        {
            transaction.Date = DateTime.Now;
            transaction.Payer = FindClientById(transaction.Payer.Id);
            transaction.Receiver = FindClientById(transaction.Receiver.Id);
        }

        private void ExecuteTransaction(Transaction transaction)
        {
            transaction.Payer.Account.Balance -= transaction.Amount.Value;
            transaction.Receiver.Account.Balance += transaction.Amount.Value;
        }

        private void SetMissingValues(Loan loan)
        {
            loan.NumberOfPaidIntallments = 0;
            loan.ApprovalDate = DateTime.Now;
        }

        private bool IsDeadlineAfterApprovalDate(Loan loan) => loan.Deadline > loan.ApprovalDate;

        private void SetNumberOfInstallments(Loan loan)
            => loan.NumberOfInstallments = CalculateNumberOfInstallments(loan);

        private long CalculateNumberOfInstallments(Loan loan) =>
            ((loan.Deadline.Year - loan.ApprovalDate.Year) * 12) + loan.Deadline.Month - loan.ApprovalDate.Month;

        private void SetInstallmentAmount(Loan loan) =>
            loan.InstallmentAmount = CalculateInstallmentAmount(loan);

        private double CalculateInstallmentAmount(Loan loan)
            => (loan.Base * (1 + loan.InterestRate / 100)) / loan.NumberOfInstallments;


        private void ApproveLoan(Loan loan) => loan.Client.Account.Balance += loan.Base;


        private bool IsAccountNumberUnique(string accountNumber)
        {
            foreach (var client in _clients)
            {
                if (client.Account.Number.Equals(accountNumber))
                    return false;

            }
            return true;
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

        private void SaveTransactions() =>
         WriteAllLinesToFile(CLIENT_FILE,
             _transactions
             .Select(ConvertEntityToCSVFormat)
             .ToList());

        private void SaveLoans() =>
           WriteAllLinesToFile(CLIENT_FILE,
               _loans
               .Select(ConvertEntityToCSVFormat)
               .ToList());

        private void SaveClients()
        {
            SaveAccounts();
            WriteAllLinesToFile(CLIENT_FILE,
                _clients
                .Select(ConvertEntityToCSVFormat)
                .ToList());
        }
            

        private void SaveAccounts() =>
            WriteAllLinesToFile(ACCOUNT_FILE,
                _accounts
                .Select(ConvertEntityToCSVFormat)
                .ToList());


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
                new Amount(double.Parse(tokens[3])),
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
            return new Account(long.Parse(tokens[0]), new Util.AccountNumber(tokens[1]), double.Parse(tokens[2]));
        }

        private void AppendLineToFile(string path, string line)
            => File.AppendAllText(path, line + Environment.NewLine);

        private void WriteAllLinesToFile(string path, List<string> content)
            => File.WriteAllLines(path, content.ToArray());
    }
}
