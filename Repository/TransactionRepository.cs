using Bank.Exception;
using Bank.Model;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Repository
{
    public class TransactionRepository
    {
        private const string NOT_FOUND_ERROR = "Transaction with {0}:{1} can not be found!";

        private readonly CSVStream<Transaction> _stream;
        private readonly ISequencer<long> _sequencer;

        public TransactionRepository(CSVStream<Transaction> stream, ISequencer<long> sequencer)
        {
            _stream = stream;
            _sequencer = sequencer;
            _sequencer.Initialize(GetMaxId(_stream.ReadAll()));
        }

        public IEnumerable<Transaction> GettAll() => _stream.ReadAll();

        public Transaction Get(long id)
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

        public Transaction Create(Transaction transaction)
        {
            transaction.Id = GenerateClientId();
            _stream.AppendToFile(transaction);
            return transaction;
        }

        public void Update(Transaction transaction)
        {
            try
            {
                var transactions = _stream.ReadAll().ToList();
                transactions[transactions.FindIndex(clt => clt.Id == transaction.Id)] = transaction;
                _stream.SaveAll(transactions);
            }
            catch (ArgumentException)
            {
                ThrowEntityNotFoundException("id", transaction.Id);
            }
        }

        public void Delete(Transaction transaction)
        {
            var transactions = _stream.ReadAll().ToList();
            var transactionToRemove = transactions.SingleOrDefault(tr => tr.Id == transaction.Id);
            if (transactionToRemove != null)
            {
                transactions.Remove(transactionToRemove);
                _stream.SaveAll(transactions);
            }
            else
            {
                ThrowEntityNotFoundException("id", transaction.Id);
            }
        }

        private long GetMaxId(IEnumerable<Transaction> transactions)
            => transactions.Count() == 0 ? 0 : transactions.Max(clt => clt.Id);

        private long GenerateClientId() => _sequencer.GenerateId();

        private void ThrowEntityNotFoundException(string key, object value)
           => throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, key, value));
    }
}
