using Bank.Model;
using Bank.Model.Util;

namespace Bank.Repository.CSV.Converter
{
    public class AccountCSVConverter : ICSVConverter<Account>
    {
        private readonly string _delimiter;

        public AccountCSVConverter(string delimiter)
        {
            _delimiter = delimiter;
        }

        public string ConvertEntityToCSVFormat(Account account)
          => string.Join(_delimiter,
              account.Id,
              account.Number,
              account.Balance);

        public Account ConvertCSVFormatToEntity(string acountCSVFormat)
        {
            string[] tokens = acountCSVFormat.Split(_delimiter.ToCharArray());
            return new Account(
                long.Parse(tokens[0]),
                new AccountNumber(tokens[1]),
                double.Parse(tokens[2]));
        }
    }
}
