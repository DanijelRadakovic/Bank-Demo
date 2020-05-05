using Bank.View.Converter;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace Bank.View.Model
{
    /// <summary>
    /// Interaction logic for DataViewer.xaml
    /// </summary>
    public partial class DataView : UserControl
    {
        private readonly Bank.Model.Bank bank;
        public ObservableCollection<UserControl> Data { get; set; }
        public DataView()
        {
            InitializeComponent();
            DataContext = this;

            bank = Bank.Model.Bank.GetInstance();

            Data = new ObservableCollection<UserControl>(TransactionConverter
                .ConvertTransactionListToTransactionViewList(bank.Transactions));

            LoanConverter
                .ConvertLoanListToLoanViewList(bank.Loans)
                .ToList()
                .ForEach(Data.Add);
        }
    }
}
