using Bank.Exception;
using Bank.Model;
using Bank.View.Converter;
using Bank.View.Model;
using System;
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
    /// Interaction logic for AddLoanDialog.xaml
    /// </summary>
    public partial class AddLoanDialog : Window
    {
        private const string ERROR_MESSAGE = "All fields are mandatory, please fill them!";
        private const string DATE_FORMAT_ERROR_MESSAGE = "Invalid date format. Valid format is: 15.05.2020. !";
        private const string INVALID_DEADLINE_ERROR_MESSAGE = "Invalid deadline value. Valid date starts from next month!";

        private static readonly Regex _decimalRegex = new Regex("[^0-9.-]+");

        private DateTime _deadlineLowerLimit;

        private Bank.Model.Bank _bank;
        private DataView _dataView;
        private IList<Client> _clients;
        private double _base;
        private double _interestRate;
        private DateTime _deadline;

        public ObservableCollection<string> Accounts { get; set; }

        public AddLoanDialog()
        {
            InitializeComponent();
            DataContext = this;

            _deadlineLowerLimit = DateTime.Now.AddMonths(1);
            _bank = Bank.Model.Bank.GetInstance();
            _dataView = (Application.Current.MainWindow as MainWindow).GetDataView();

            _clients = _bank.Clients;
            Accounts = new ObservableCollection<string>(FindAccountNumbersFromClients());

            Deadline.Text = DateTime.Now.AddYears(1).ToString("dd.MM.yyyy.");
        }

        public double Base
        {
            get { return _base; }
            set
            {
                if (_base != value)
                {
                    _base = value;
                    OnPropertyChanged();
                }
            }
        }

        public double InterestRate
        {
            get { return _interestRate; }
            set
            {
                if (_interestRate != value)
                {
                    _interestRate = value;
                    OnPropertyChanged();
                }
            }
        }

        private IList<string> FindAccountNumbersFromClients()
            => _clients
            .Select(client => client.Account.Number)
            .ToList();

        private Client FindClientFromAccountNumber(string accountNumber)
            => _clients
            .Where(client => client.Account.Number.Equals(accountNumber))
            .First();

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidDeadlineFormatDate())
            {
                ShowError(DATE_FORMAT_ERROR_MESSAGE);
            }
            else if (IsDeadlineLowerThanLowerLimit())
            {
                ShowError(INVALID_DEADLINE_ERROR_MESSAGE);
            }
            else if (!IsClientSelected())
            {
                ShowError(ERROR_MESSAGE);
            }
            else
            {
                try
                {
                    UpdateDataView(CreateLoan());
                    CloseDialog();
                }
                catch (InvalidLoanDeadline error)
                {
                    ShowError(error.Message);
                }

            }
        }

        private Loan CreateLoan()
        {
            try
            {
                return _bank.Create(new Loan(
                      FindClientFromAccountNumber(Client.SelectedItem.ToString()),
                      _deadline,
                      _base,
                      _interestRate));
            }
            catch (InvalidLoanDeadline)
            {
                throw;
            }
        }


        private void UpdateDataView(Loan loan) => _dataView.Data.Add(LoanConverter.ConvertLoanToLoanView(loan));

        private void CloseDialog() => Close();


        private bool IsClientSelected() => Client.SelectedItem != null;

        private bool IsValidDeadlineFormatDate()
        {
            if (!DateTime.TryParse(Deadline.Text, out DateTime deadline))
            {
                return false;
            }
            else
            {
                _deadline = deadline;
                return true;
            }

        }

        private bool IsDeadlineLowerThanLowerLimit() => _deadline < _deadlineLowerLimit;


        private void ShowError(string s) => new MessageDialog(s, this).ShowDialog();

        private void InterestRateTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (double.TryParse(textbox.Text, out double value))
            {
                if (value > 100)
                    textbox.Text = "100";
                else if (value < 0)
                    textbox.Text = "0";
            }
        }

        private void MaskDecimalInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextDecimal(e.Text);
        }

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


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
