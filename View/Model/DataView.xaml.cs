using Bank.model;
using Bank.View.Converter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Bank.View.Model
{
    /// <summary>
    /// Interaction logic for DataViewer.xaml
    /// </summary>
    public partial class DataView : UserControl
    {

        public ObservableCollection<UserControl> Data { get; set; }
        public DataView()
        {
            InitializeComponent();
            DataContext = this;

            model.Bank bank = new model.Bank(
                id: 0L,
                name: "Crypto Bank",
                account: new Account(0L, "836-788951889-15", 100000),
                clients: new List<Client>(),
                transactions: new List<Transaction>(),
                loans: new List<Loan>()
                );
                

            Loan loan = new Loan(
                id: 0L,
                client: new Client(0L, "Joy", "Jordyson", DateTime.Now, new Account(0L, "170-256484612-75", 5000)),
                approvalDate: DateTime.Now,
                deadline: DateTime.Now.AddYears(1),
                @base: 3000.56,
                interestRate: 5.73,
                numberOfInstallments: 12,
                installmentAmount: 300,
                numberOfPaidIntallments: 0,
                bank: bank);

            Transaction transaction = new Transaction(
                id: 0L,
                purpose: "Money transfer",
                date: DateTime.Now,
                amount: 1000,
                payer: new Client(0L, "John", "Johnson", DateTime.Now, new Account(0L, "756-865851841-63", 7000)),
                receiver: new Client(0L, "Peter", "Peterson", DateTime.Now, new Account(0L, "423-785418413-74", 4000)),
                commissionFee: 100,
                bank: bank);

           

            Data = new ObservableCollection<UserControl>
            {
                TransactionConverter.ConvertTransactionToTransactionView(transaction),
                LoanConverter.ConvertLoanToLoanView(loan),
            };

            
        }
    }
}
