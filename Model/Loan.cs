using System;

namespace Bank.model
{
    class Loan
    {
        public long Id { get; set; }
        public Client Client { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime Deadline { get; set; }
        public double Base { get; set; }
        public double InterestRate { get; set; }
        public long NumberOfInstallments { get; set; }
        public double InstallmentAmount { get; set; }
        public long NumberOfPaidIntallments { get; set; }

        public Bank Bank { get; set; }

        public Loan(long id, Client client, DateTime approvalDate, DateTime deadline,
            double @base, double interestRate, long numberOfInstallments,
            double installmentAmount, long numberOfPaidIntallments, Bank bank)
        {
            Id = id;
            Client = client;
            ApprovalDate = approvalDate;
            Deadline = deadline;
            Base = @base;
            InterestRate = interestRate;
            NumberOfInstallments = numberOfInstallments;
            InstallmentAmount = installmentAmount;
            NumberOfPaidIntallments = numberOfPaidIntallments;
            Bank = bank;
        }
    }
}
