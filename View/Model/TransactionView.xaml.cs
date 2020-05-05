using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Bank.View.Model
{
    /// <summary>
    /// Interaction logic for Transaction.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        private DateTime _date;
        private string _purpose;
        private string _payer;
        private string _payerAccount;
        private string _receiver;
        private string _receiverAccount;
        private double _amount;
        public TransactionView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Purpose
        {
            get { return _purpose; }
            set
            {
                if (_purpose != value)
                {
                    _purpose = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Payer
        {
            get { return _payer; }
            set
            {
                if (_payer != value)
                {
                    _payer = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PayerAccount
        {
            get { return _payerAccount; }
            set
            {
                if (_payerAccount != value)
                {
                    _payerAccount = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Receiver
        {
            get { return _receiver; }
            set
            {
                if (_receiver != value)
                {
                    _receiver = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ReceiverAccount
        {
            get { return _receiverAccount; }
            set
            {
                if (_receiverAccount != value)
                {
                    _receiverAccount = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
