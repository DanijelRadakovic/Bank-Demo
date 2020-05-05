using Bank.Model;
using System;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var bank = Model.Bank.GetInstance();

            var client1 = new Client(0L, "Joy", "Jordyson", DateTime.Now, new Account(0L, "170-256484612-75", 5000));
            var client2 = new Client(1L, "Jack", "Jackson", DateTime.Now, new Account(0L, "170-774484612-38", 6000));
            var client3 = new Client(2L, "John", "Johnson", DateTime.Now, new Account(0L, "756-865851841-63", 7000));
            var client4 = new Client(3L, "Peter", "Peterson", DateTime.Now, new Account(0L, "423-785418413-74", 4000));

            var loan1 = new Loan(
                   id: 0L,
                   client: client1,
                   approvalDate: DateTime.Now,
                   deadline: DateTime.Now.AddYears(1),
                   @base: 3000.56,
                   interestRate: 5.73,
                   numberOfInstallments: 12,
                   installmentAmount: 300,
                   numberOfPaidIntallments: 0);
            var loan2 = new Loan(
                    id: 1L,
                    client: client2,
                    approvalDate: DateTime.Now,
                    deadline: DateTime.Now.AddYears(1),
                    @base: 6000,
                    interestRate: 5.73,
                    numberOfInstallments: 12,
                    installmentAmount: 700,
                    numberOfPaidIntallments: 0);



            var transaction1 = new Transaction(
                    id: 0L,
                    purpose: "Money transfer",
                    date: DateTime.Now,
                    amount: 1000,
                    payer: client3,
                    receiver: client4);
            var transaction2 = new Transaction(
                    id: 1L,
                    purpose: "Money transfer",
                    date: DateTime.Now,
                    amount: 2000,
                    payer: client3,
                    receiver: client4);


            bank.Create(client1);
            bank.Create(client2);
            bank.Create(client3);
            bank.Create(client4);

            bank.Create(loan1);
            bank.Create(loan2);

            bank.Create(transaction1);
            bank.Create(transaction2);
        }
    }
}
