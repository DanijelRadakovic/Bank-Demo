using Bank.Exception;

namespace Bank.Model.Util
{
    class Amount
    {
        private const double MINIMAL_AMOUNT = 100;
        private static readonly string ERROR_MESSAGE = $"Minimal valid amount is:{MINIMAL_AMOUNT}";

        private double _value;

        public Amount(double amount)
        {
            if (IsValidAmount(amount))
                _value = amount;
            else
                throw new ValidationException(ERROR_MESSAGE);
        }

        public double Value
        {
            get => _value;
            set
            {
                if (IsValidAmount(value))
                    _value = value;
                else
                    ThrowException();
            }
        }

        private bool IsValidAmount(double amount)
            => amount >= MINIMAL_AMOUNT;

        private void ThrowException()
           => throw new ValidationException(ERROR_MESSAGE);
    }
}
