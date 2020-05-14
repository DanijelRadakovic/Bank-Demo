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
        private readonly LoanController _loanController;
        private readonly TransactionController _transactionController;

        public ObservableCollection<UserControl> Data { get; set; }
        public DataView()
        {
            InitializeComponent();
            DataContext = this;

            var app = Application.Current as App;

            _loanController = app.LoanController;
            _transactionController = app.TransactionController;

            Data = new ObservableCollection<UserControl>(TransactionConverter
                .ConvertTransactionListToTransactionViewList(_transactionController
                    .GetAll()
                    .ToList()));

            LoanConverter
                .ConvertLoanListToLoanViewList(_loanController
                    .GetAll()
                    .ToList())
                .ToList()
                .ForEach(Data.Add);
        }
    }
}
