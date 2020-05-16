using Bank.Model;
using System;

namespace Bank.Repository.CSV.Converter
{
    public class LoanCSVConverter : ICSVConverter<Loan>
    {
        private readonly string _delimiter;
        private readonly string _datetimeFormat;

        public LoanCSVConverter(string delimiter, string datetimeFormat)
        {
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
        }

        public string ConvertEntityToCSVFormat(Loan loan)
           => string.Join(_delimiter,
               loan.Id,
               loan.Client.Id,
               loan.ApprovalDate.ToString(_datetimeFormat),
               loan.Deadline.ToString(_datetimeFormat),
               loan.Base,
               loan.InterestRate,
               loan.NumberOfInstallments,
               loan.InstallmentAmount,
               loan.NumberOfPaidIntallments);

        public Loan ConvertCSVFormatToEntity(string loanCSVFormat)
        {
            string[] tokens = loanCSVFormat.Split(_delimiter.ToCharArray());
            return new Loan(
                long.Parse(tokens[0]),
                new Client(long.Parse(tokens[1])),
                DateTime.Parse(tokens[2]),
                DateTime.Parse(tokens[3]),
                double.Parse(tokens[4]),
                double.Parse(tokens[5]),
                long.Parse(tokens[6]),
                double.Parse(tokens[7]),
                long.Parse(tokens[8]));
        }
    }
}
