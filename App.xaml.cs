using Bank.Controller;
using Bank.Model;
using Bank.Repository;
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
            var accountRepository = new AccountRepository(ACCOUNT_FILE, CSV_DELIMITER);
            var clientRepository = new ClientRepository(CLIENT_FILE, CSV_DELIMITER, DATETIME_FORMAT);
            var loanRepository = new LoanRepository(LOAN_FILE, CSV_DELIMITER, DATETIME_FORMAT);

            var accountService = new AccountService(accountRepository);
            var clientService = new ClientService(clientRepository, accountService);
            var loanService = new LoanService(loanRepository, clientService);

            AccountController = new AccountController(accountService);
            ClientController = new ClientController(clientService);
            LoanController = new LoanController(loanService);
        }

        public AccountController AccountController { get; private set; }
        public ClientController ClientController { get; private set; }
        public LoanController LoanController { get; private set; }
    }
}
