using Bank.Exception;
using Bank.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank.Repository
{
    public class LoanRepository
    {
        private const string NOT_FOUND_ERROR = "Loan with {0}:{1} can not be found!";

        private readonly string _path;
        private readonly string _delimiter;
        private readonly string _datetimeFormat;
        private long _loanNextId;

        public LoanRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
            InitializeId();
        }

        public IEnumerable<Loan> GettAll() => ReadAll();

        public Loan Get(long id)
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

        public Loan Create(Loan loan)
        {
            loan.Id = GenerateClientId();
            AppendLineToFile(_path, ConvertEntityToCSVFormat(loan));
            return loan;
        }

        public void Update(Loan loan)
        {
            try
            {
                var loans = ReadAll().ToList();
                loans[loans.FindIndex(clt => clt.Id == loan.Id)] = loan;
                SaveAll(loans);
            }
            catch (ArgumentException)
            {
                ThrowEntityNotFoundException("id", loan.Id);
            }
        }

        public void Delete(Loan loan)
        {
            var loans = ReadAll().ToList();
            var loanToRemove = loans.SingleOrDefault(ln => ln.Id == loan.Id);
            if (loanToRemove != null)
            {
                loans.Remove(loanToRemove);
                SaveAll(loans);
            }
            else
            {
                ThrowEntityNotFoundException("id", loan.Id);
            }
        }

        private void InitializeId() => _loanNextId = GetMaxId(ReadAll());

        private long GetMaxId(IEnumerable<Loan> loans)
            => loans.Count() == 0 ? 0 : loans.Max(clt => clt.Id);

        private long GenerateClientId() => _loanNextId++;

        private void ThrowEntityNotFoundException(string key, object value)
           => throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, key, value));

        private string ConvertEntityToCSVFormat(Loan loan)
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

        private Loan ConvertCSVFormatToEntity(string loanCSVFormat)
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

        private IEnumerable<Loan> ReadAll()
            => File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToEntity)
                .ToList();

        private void SaveAll(IEnumerable<Loan> loans)
            => WriteAllLinesToFile(_path,
                 loans
                 .Select(ConvertEntityToCSVFormat)
                 .ToList());

        private void AppendLineToFile(string path, string line)
           => File.AppendAllText(path, line + Environment.NewLine);

        private void WriteAllLinesToFile(string path, IEnumerable<string> content)
            => File.WriteAllLines(path, content.ToArray());
    }
}
