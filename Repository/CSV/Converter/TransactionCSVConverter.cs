using Bank.Model;
using Bank.Model.Util;
using System;

namespace Bank.Repository.CSV.Converter
{
    public class TransactionCSVConverter : ICSVConverter<Transaction>
    {
        private readonly string _delimiter;
        private readonly string _datetimeFormat;

        public TransactionCSVConverter(string delimiter, string datetimeFormat)
        {
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
        }

        public string ConvertEntityToCSVFormat(Transaction transaction)
            => string.Join(_delimiter,
                transaction.Id,
                transaction.Purpose,
                transaction.Date.ToString(_datetimeFormat),
                transaction.Amount,
                transaction.Payer.Id,
                transaction.Receiver.Id);

        public Transaction ConvertCSVFormatToEntity(string transactionCSVFormat)
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
    }
}
