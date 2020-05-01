using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.model
{
    class Loan
    {
        internal Client Client { get; set; }
        public DateTime RepaymentEndDate { get; set; }
        public DateTime ApprovalDate { get; set; }
        public double Base { get; set; }
        public double InterestRate { get; set; }
        public long NumberOfInstallments { get; set; }
        public double InstallmentAmount { get; set; }
        public long NumberOfPaidIntallments { get; set; }

        public Loan(Client client, DateTime repaymentEndDate, DateTime approvalDate, 
            double @base, double interestRate, long numberOfInstallments, 
            double installmentAmount, long numberOfPaidIntallments)
        {
            Client = client;
            RepaymentEndDate = repaymentEndDate;
            ApprovalDate = approvalDate;
            Base = @base;
            InterestRate = interestRate;
            NumberOfInstallments = numberOfInstallments;
            InstallmentAmount = installmentAmount;
            NumberOfPaidIntallments = numberOfPaidIntallments;
        }
    }
}
