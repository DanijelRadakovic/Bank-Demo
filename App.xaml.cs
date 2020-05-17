using Bank.Controller;
using Bank.Model;
using Bank.Repository;
using Bank.Repository.CSV.Converter;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;
using Bank.Service;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CLIENT_FILE = "../../Resources/Data/clients.csv";
        private const string ACCOUNT_FILE = "../../Resources/Data/accounts.csv";
        private const string LOAN_FILE = "../../Resources/Data/loans.csv";
        private const string TRANSACTION_FILE = "../../Resources/Data/transactions.csv";
        private const string CSV_DELIMITER = ",";

        private const string DATETIME_FORMAT = "dd.MM.yyyy.";

        public App()
        {
            var accountRepository = new AccountRepository(
                new CSVStream<Account>(ACCOUNT_FILE, new AccountCSVConverter(CSV_DELIMITER)),
                new LongSequencer());
            var clientRepository = new ClientRepository(
                new CSVStream<Client>(CLIENT_FILE, new ClientCSVConverter(CSV_DELIMITER, DATETIME_FORMAT)),
                new LongSequencer());
            var loanRepository = new LoanRepository(
                new CSVStream<Loan>(LOAN_FILE, new LoanCSVConverter(CSV_DELIMITER, DATETIME_FORMAT)),
                new LongSequencer());
            var transactionRepository = new TransactionRepository(
                new CSVStream<Transaction>(TRANSACTION_FILE, new TransactionCSVConverter(CSV_DELIMITER, DATETIME_FORMAT)),
                new LongSequencer());

            var accountService = new AccountService(accountRepository);
            var clientService = new ClientService(clientRepository, accountService);
            var loanService = new LoanService(loanRepository, clientService);
            var transactionService = new TransactionService(transactionRepository, clientService);

            AccountController = new AccountController(accountService);
            ClientController = new ClientController(clientService);
            LoanController = new LoanController(loanService);
            TransactionController = new TransactionController(transactionService);
        }

        public IController<Account, long> AccountController { get; private set; }
        public IController<Client, long> ClientController { get; private set; }
        public IController<Loan, long> LoanController { get; private set; }
        public IController<Transaction, long> TransactionController { get; private set; }
    }
}
