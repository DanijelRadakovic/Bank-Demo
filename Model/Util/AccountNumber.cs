using Bank.Exception;
using System.Text.RegularExpressions;

namespace Bank.Model.Util
{
    class AccountNumber
    {
        private readonly Regex _regex = new Regex(@"\d{3}-\d{11}-\d{2}");
        private const string ERROR_MESSAGE = "Invalid account number value. Acceptable format is 745-56324786314-35";

        private string _value;

        public AccountNumber(string accountNumber)
        {
            if (IsValidNumberFormat(accountNumber))
                _value = accountNumber;
            else
                ThrowException();
        }

        public string Value
        {
            get => _value;
            set
            {
                if (IsValidNumberFormat(value))
                    _value = value;
                else
                    ThrowException();
            }
        }

        private bool IsValidNumberFormat(string accountNumber)
            => _regex.Match(accountNumber).Success;

        private void ThrowException()
            => throw new ValidationException(ERROR_MESSAGE);
    }
}
