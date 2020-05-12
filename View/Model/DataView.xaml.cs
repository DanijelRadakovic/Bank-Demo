using Bank.Controller;
using Bank.View.Converter;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Bank.View.Model
{
    /// <summary>
    /// Interaction logic for DataViewer.xaml
    /// </summary>
    public partial class DataView : UserControl
    {
        private readonly Bank.Model.Bank bank;

        private readonly LoanController _loanController;

        public ObservableCollection<UserControl> Data { get; set; }
        public DataView()
        {
            InitializeComponent();
            DataContext = this;

            var app = Application.Current as App;

            bank = Bank.Model.Bank.GetInstance();
            _loanController = app.LoanController;

            Data = new ObservableCollection<UserControl>(TransactionConverter
                .ConvertTransactionListToTransactionViewList(bank.Transactions));

            LoanConverter
                .ConvertLoanListToLoanViewList(_loanController
                    .GetAll()
                    .ToList())
                .ToList()
                .ForEach(Data.Add);
        }
    }
}
