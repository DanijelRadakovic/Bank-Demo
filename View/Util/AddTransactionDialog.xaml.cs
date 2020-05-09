using Bank.Model;
using Bank.Model.Util;
using Bank.View.Converter;
using Bank.View.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Bank.View.Util
{
    /// <summary>
    /// Interaction logic for CreateTransactionDialog.xaml
    /// </summary>
    public partial class AddTransactionDialog : Window
    {
        private const string ERROR_MESSAGE = "All fields are mandatory, please fill them!";
        private static readonly Regex _decimalRegex = new Regex("[^0-9.-]+");


        private readonly Bank.Model.Bank _bank;
        private readonly DataView _dataView;
        private readonly IList<Client> _clients;
        private string _purpose;
        private double _amount;


        public ObservableCollection<string> PayerAccounts { get; set; }
        public ObservableCollection<string> ReceiverAccounts { get; set; }


        public AddTransactionDialog()
        {
            InitializeComponent();
            DataContext = this;

            _bank = Bank.Model.Bank.GetInstance();
            _clients = _bank.Clients;
            _dataView = (Application.Current.MainWindow as MainWindow).GetDataView();

            PayerAccounts = new ObservableCollection<string>(FindAccountNumbersFromClients());
            ReceiverAccounts = new ObservableCollection<string>(FindAccountNumbersFromClients());
        }

        public string Purpose
        {
            get { return _purpose; }
            set
            {
                if (_purpose != value)
                {
                    _purpose = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }

        private IList<string> FindAccountNumbersFromClients()
            => _clients
            .Select(client => client.Account.Number.Value)
            .ToList();

        private Client FindClientFromAccountNumber(string accountNumber)
            => _clients
            .Where(client => client.Account.Number.Equals(accountNumber))
            .First();


        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            if (AreAccountsSelected(PayerAccount, ReceiverAccount))
            {
                var transaction = CreateTransaction();
                UpdateDataView(transaction);
                CloseDialog();
            }
            else
            {
                ShowError();
            }
        }

        private Transaction CreateTransaction()
        {
            var transaction = new Transaction(
                _purpose,
                new Amount(_amount),
                FindClientFromAccountNumber(PayerAccount.SelectedItem.ToString()),
                FindClientFromAccountNumber(ReceiverAccount.SelectedItem.ToString()));
            return _bank.Create(transaction);
        }

        private void UpdateDataView(Transaction transaction) 
            => _dataView.Data.Add(TransactionConverter.ConvertTransactionToTransactionView(transaction));

        private void CloseDialog() => Close();

        private void ShowError() => new MessageDialog(ERROR_MESSAGE, this).ShowDialog();

        private bool AreAccountsSelected(ComboBox payer, ComboBox receiver)
            => IsAccountSelected(payer) && IsAccountSelected(receiver);

        private bool IsAccountSelected(ComboBox comboBox)
            => comboBox.SelectedItem != null;

        private void MaskDecimaInput(object sender, TextCompositionEventArgs e)
            => e.Handled = !IsTextDecimal(e.Text);

        private void MaskDecimalPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string input = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextDecimal(input)) e.CancelCommand();
            }
            else
            {
                e.CancelCommand();
            }
        }

        private bool IsTextDecimal(string input) => !_decimalRegex.IsMatch(input);

        private void PayerAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox payerComboBox = sender as ComboBox;
            ReceiverAccounts.Remove(payerComboBox.SelectedItem.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
