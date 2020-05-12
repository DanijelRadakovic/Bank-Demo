using Bank.Exception;

namespace Bank.Model.Util
{
    public class Amount
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

        public override bool Equals(object obj)
        {
            return obj is Amount amount &&
                   _value == amount._value &&
                   Value == amount.Value;
        }

        public override int GetHashCode()
        {
            int hashCode = 1571931217;
            hashCode = hashCode * -1521134295 + _value.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        private bool IsValidAmount(double amount)
            => amount >= MINIMAL_AMOUNT;

        private void ThrowException()
           => throw new ValidationException(ERROR_MESSAGE);
    }
}
