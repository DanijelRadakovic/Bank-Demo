using Bank.Exception;
using Bank.Model;
using Bank.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Service
{
    public class LoanService
    {
        private const string INVALID_DATE_ERROR = "Deadline: {loan.Deadline} is before approval date: {loan.ApprovalDate}";

        private readonly LoanRepository loanRepository;
        private readonly ClientService clientService;

        public LoanService(LoanRepository loanRepository, ClientService clientService)
        {
            this.loanRepository = loanRepository;
            this.clientService = clientService;
        }

        public IEnumerable<Loan> GetAll()
        {
            var clients = clientService.GetAll();
            var loans = loanRepository.GettAll();
            BindClientsWithLoans(clients, loans);
            return loans;
        }

        public Loan Get(long id)
        {
            var loan = loanRepository.Get(id);
            loan.Client = clientService.Get(loan.Client.Id);
            return loan;
        }

        public Loan Create(Loan loan)
        {
            Client client;
            Loan newLoan;

            SetMissingValues(loan);
            if (IsDeadlineAfterApprovalDate(loan))
            {
                SetNumberOfInstallments(loan);
                SetInstallmentAmount(loan);
                ApproveLoan(loan);

                client = clientService.Create(loan.Client);
                newLoan = loanRepository.Create(loan);
                newLoan.Client = client;

                return newLoan;
            }
            else
            {
                throw new InvalidDateException(string.Format(INVALID_DATE_ERROR, 
                    loan.Deadline, loan.ApprovalDate));
            }
        }

        public void Update(Loan loan)
        {
            clientService.Update(loan.Client);
            loanRepository.Update(loan);
        }

        public void Delete(Loan loan)
        {
            clientService.Delete(loan.Client);
            loanRepository.Delete(loan);
        }

        private void SetMissingValues(Loan loan)
        {
            loan.NumberOfPaidIntallments = 0;
            loan.ApprovalDate = DateTime.Now;
        }

        private bool IsDeadlineAfterApprovalDate(Loan loan) => loan.Deadline > loan.ApprovalDate;

        private void SetNumberOfInstallments(Loan loan)
            => loan.NumberOfInstallments = CalculateNumberOfInstallments(loan);

        private long CalculateNumberOfInstallments(Loan loan) =>
            ((loan.Deadline.Year - loan.ApprovalDate.Year) * 12) + loan.Deadline.Month - loan.ApprovalDate.Month;

        private void SetInstallmentAmount(Loan loan) =>
            loan.InstallmentAmount = CalculateInstallmentAmount(loan);

        private double CalculateInstallmentAmount(Loan loan)
            => (loan.Base * (1 + loan.InterestRate / 100)) / loan.NumberOfInstallments;

        private void ApproveLoan(Loan loan) => loan.Client.Account.Balance += loan.Base;

        private void BindClientsWithLoans(IEnumerable<Client> clients, IEnumerable<Loan> loans)
            => loans.ToList().ForEach(loan => loan.Client = FindClientById(clients, loan.Client.Id));

        private Client FindClientById(IEnumerable<Client> clients, long id)
            => clients.SingleOrDefault(client => client.Id == id);
    }
}
