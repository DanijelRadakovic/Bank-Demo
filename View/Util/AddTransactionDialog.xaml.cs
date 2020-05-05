using Bank.View.Converter;
using System.Collections.ObjectModel;
using System.Windows;

namespace Bank.View.Util
{
    /// <summary>
    /// Interaction logic for CreateTransactionDialog.xaml
    /// </summary>
    public partial class AddTransactionDialog : Window
    {

        public ObservableCollection<string> Payers { get; set; }
        public ObservableCollection<string> Receivers { get; set; }

        private Bank.Model.Bank bank;

        public AddTransactionDialog()
        {
            InitializeComponent();
            DataContext = this;

            bank = Bank.Model.Bank.GetInstance();

            Payers = new ObservableCollection<string>(ClientConverter.ConvertClientListToStringList(bank.Clients));
            Receivers = new ObservableCollection<string>(ClientConverter.ConvertClientListToStringList(bank.Clients));
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
