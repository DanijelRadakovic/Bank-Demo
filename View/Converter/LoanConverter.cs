using Bank.model;
using Bank.View.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.View.Converter
{
    class LoanConverter : AbstractConverter
    {
        public static LoanView ConvertLoanToLoanView(Loan loan)
            => new LoanView
            {
                ApprovalDate = loan.ApprovalDate,
                Deadline = loan.Deadline,
                Client = loan.Client.FirstName + " " + loan.Client.LastName,
                ClientAccount = loan.Client.Account.Number,
                Base = loan.Base,
                InterestRate = loan.InterestRate,
                NumberOfInstallments = loan.NumberOfInstallments,
                InstallmentAmount = loan.InstallmentAmount,
                NumberOfPaidInstallments = loan.NumberOfPaidIntallments
            };

        
        public static IList<LoanView> ConvertLoanListToLoanViewList(IList<Loan> loans)
            => ConvertEntityListToViewList(loans, ConvertLoanToLoanView);
    }
}
