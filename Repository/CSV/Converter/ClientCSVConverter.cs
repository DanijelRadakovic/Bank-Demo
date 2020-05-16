using Bank.Model;
using System;

namespace Bank.Repository.CSV.Converter
{
    public class ClientCSVConverter : ICSVConverter<Client>
    {
        private readonly string _delimiter;
        private readonly string _datetimeFormat;

        public ClientCSVConverter(string delimiter, string datetimeFormat)
        {
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
        }

        public string ConvertEntityToCSVFormat(Client client)
           => string.Join(_delimiter,
               client.Id,
               client.FirstName,
               client.LastName,
               client.DateOfBirth.ToString(_datetimeFormat),
               client.Account.Id);

        public Client ConvertCSVFormatToEntity(string clientCSVFormat)
        {
            string[] tokens = clientCSVFormat.Split(_delimiter.ToCharArray());
            return new Client(
                long.Parse(tokens[0]),
                tokens[1], tokens[2],
                DateTime.Parse(tokens[3]),
                new Account(long.Parse(tokens[4])));
        }
    }
}
