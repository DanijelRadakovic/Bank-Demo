using Bank.Exception;
using Bank.Model;
using Bank.Model.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank.Repository
{
    public class TransactionRepository
    {
        private const string NOT_FOUND_ERROR = "Transaction with {0}:{1} can not be found!";

        private readonly string _path;
        private readonly string _delimiter;
        private readonly string _datetimeFormat;
        private long _transactionNextId;

        public TransactionRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
            InitializeId();
        }

        public IEnumerable<Transaction> GettAll() => ReadAll();

        public Transaction Get(long id)
        {
            try
            {
                return ReadAll().SingleOrDefault(ln => ln.Id == id);
            }
            catch (ArgumentException)
            {
                throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, "id", id));
            }

        }

        public Transaction Create(Transaction transaction)
        {
            transaction.Id = GenerateClientId();
            AppendLineToFile(ConvertEntityToCSVFormat(transaction));
            return transaction;
        }

        public void Update(Transaction transaction)
        {
            try
            {
                var transactions = ReadAll().ToList();
                transactions[transactions.FindIndex(clt => clt.Id == transaction.Id)] = transaction;
                SaveAll(transactions);
            }
            catch (ArgumentException)
            {
                ThrowEntityNotFoundException("id", transaction.Id);
            }
        }

        public void Delete(Transaction transaction)
        {
            var transactions = ReadAll().ToList();
            var transactionToRemove = transactions.SingleOrDefault(tr => tr.Id == transaction.Id);
            if (transactionToRemove != null)
            {
                transactions.Remove(transactionToRemove);
                SaveAll(transactions);
            }
            else
            {
                ThrowEntityNotFoundException("id", transaction.Id);
            }
        }

        private void InitializeId() => _transactionNextId = GetMaxId(ReadAll());

        private long GetMaxId(IEnumerable<Transaction> transactions)
            => transactions.Count() == 0 ? 0 : transactions.Max(clt => clt.Id);

        private long GenerateClientId() => _transactionNextId++;

        private void ThrowEntityNotFoundException(string key, object value)
           => throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, key, value));

        private string ConvertEntityToCSVFormat(Transaction transaction)
            => string.Join(_delimiter,
                transaction.Id,
                transaction.Purpose,
                transaction.Date.ToString(_datetimeFormat),
                transaction.Amount,
                transaction.Payer.Id,
                transaction.Receiver.Id);

        private Transaction ConvertCSVFormatToEntity(string transactionCSVFormat)
        {
            string[] tokens = transactionCSVFormat.Split(_delimiter.ToCharArray());
            return new Transaction(
                long.Parse(tokens[0]),
                tokens[1],
                DateTime.Parse(tokens[2]),
                new Amount(double.Parse(tokens[3])),
                new Client(long.Parse(tokens[4])),
                new Client(long.Parse(tokens[5])));
        }

        private IEnumerable<Transaction> ReadAll()
           => File.ReadAllLines(_path)
               .Select(ConvertCSVFormatToEntity)
               .ToList();

        private void SaveAll(IEnumerable<Transaction> transactions)
            => WriteAllLinesToFile(
                 transactions
                 .Select(ConvertEntityToCSVFormat)
                 .ToList());

        private void AppendLineToFile(string line)
           => File.AppendAllText(_path, line + Environment.NewLine);

        private void WriteAllLinesToFile(IEnumerable<string> content)
            => File.WriteAllLines(_path, content.ToArray());
    }
}
