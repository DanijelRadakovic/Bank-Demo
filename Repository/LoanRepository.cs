using Bank.Exception;
using Bank.Model;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Repository
{
    public class LoanRepository : IRepository<Loan, long>
    {
        private const string NOT_FOUND_ERROR = "Loan with {0}:{1} can not be found!";

        private readonly ICSVStream<Loan> _stream;
        private readonly ISequencer<long> _sequencer;

        public LoanRepository(CSVStream<Loan> stream, ISequencer<long> sequencer)
        {
            _stream = stream;
            _sequencer = sequencer;
            _sequencer.Initialize(GetMaxId(_stream.ReadAll()));
        }

        public IEnumerable<Loan> GetAll() => _stream.ReadAll();

        public Loan Get(long id)
        {
            try
            {
                return _stream.ReadAll().SingleOrDefault(ln => ln.Id == id);
            }
            catch (ArgumentException)
            {
                throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, "id", id));
            }

        }

        public Loan Create(Loan loan)
        {
            loan.Id = GenerateClientId();
            _stream.AppendToFile(loan);
            return loan;
        }

        public void Update(Loan loan)
        {
            try
            {
                var loans = _stream.ReadAll().ToList();
                loans[loans.FindIndex(clt => clt.Id == loan.Id)] = loan;
                _stream.SaveAll(loans);
            }
            catch (ArgumentException)
            {
                ThrowEntityNotFoundException("id", loan.Id);
            }
        }

        public void Delete(Loan loan)
        {
            var loans = _stream.ReadAll().ToList();
            var loanToRemove = loans.SingleOrDefault(ln => ln.Id == loan.Id);
            if (loanToRemove != null)
            {
                loans.Remove(loanToRemove);
                _stream.SaveAll(loans);
            }
            else
            {
                ThrowEntityNotFoundException("id", loan.Id);
            }
        }

        private long GetMaxId(IEnumerable<Loan> loans)
            => loans.Count() == 0 ? 0 : loans.Max(clt => clt.Id);

        private long GenerateClientId() => _sequencer.GenerateId();

        private void ThrowEntityNotFoundException(string key, object value)
           => throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, key, value));
    }
}
