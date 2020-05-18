using Bank.Model;
using Bank.Repository.Abstract;
using Bank.Repository.CSV;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Repository
{
    public class LoanRepository : CSVRepository<Loan, long>, 
        ILoanRepository,
        IEagerCSVRepository<Loan, long>
    {
        private const string ENTITY_NAME = "Loan";
        private readonly IEagerCSVRepository<Client, long> _clientRepository;

        public LoanRepository(ICSVStream<Loan> stream, 
            ISequencer<long> sequencer,
            IEagerCSVRepository<Client, long> clientRepository)
            : base(ENTITY_NAME, stream, sequencer)
        {
            _clientRepository = clientRepository;
        }

        public new IEnumerable<Loan> Find(Func<Loan, bool> predicate)
            => GetAllEager().Where(predicate);

        public IEnumerable<Loan> GetAllEager()
        {
            var clients = _clientRepository.GetAllEager();
            var loans = GetAll();
            BindClientsWithLoans(clients, loans);
            return loans;
        }

        public Loan GetEager(long id)
        {
            var loan = Get(id);
            loan.Client = _clientRepository.GetEager(loan.Client.Id);
            return loan;
        }

        public IEnumerable<Loan> GetByBaseOrderByAscWithPagination(int pageIndex, int pageSize = 10)
        {
            var loans = GetAll();
            loans.ToList()
                .Sort((loan1, loan2) => loan1.Base.CompareTo(loan2.Base));
            return loans.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);
        }

        public IEnumerable<Loan> GetByInterestRateGreaterThanAndClientBalanceGreaterThan(
            double interestRate, double clientBalance)
            => Find(loan => loan.InterestRate > interestRate && loan.Client.Account.Balance > clientBalance);

        private void BindClientsWithLoans(IEnumerable<Client> clients, IEnumerable<Loan> loans)
            => loans.ToList().ForEach(loan => loan.Client = FindClientById(clients, loan.Client.Id));

        private Client FindClientById(IEnumerable<Client> clients, long id)
            => clients.SingleOrDefault(client => client.Id == id);
    }
}
