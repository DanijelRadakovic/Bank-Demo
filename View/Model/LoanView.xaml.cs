using Bank.model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Bank.View.Model
{
    /// <summary>
    /// Interaction logic for Loan.xaml
    /// </summary>
    public partial class LoanView : UserControl
    {
        private DateTime _approvalDate;
        private DateTime _deadline;
        private string _client;
        private string _clientAccount;
        private double _base;
        private double _interestRate;
        private long _numberOfInstallments;
        private double _installmentsAmount;
        private long _numberOfPaidInstallments;
        

        public LoanView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DateTime ApprovalDate
        {
            get { return _approvalDate; }
            set
            {
                if (_approvalDate != value)
                {
                    _approvalDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Deadline
        {
            get { return _deadline; }
            set
            {
                if (_deadline != value)
                {
                    _deadline = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Client
        {
            get { return _client; }
            set
            {
                if (_client != value)
                {
                    _client = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ClientAccount
        {
            get { return _clientAccount; }
            set
            {
                if (_clientAccount != value)
                {
                    _clientAccount = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Base
        {
            get { return _base; }
            set
            {
                if (_base != value)
                {
                    _base = value;
                    OnPropertyChanged();
                }
            }
        }

        public double InterestRate
        {
            get { return _interestRate; }
            set
            {
                if (_interestRate != value)
                {
                    _interestRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public long NumberOfInstallments
        {
            get { return _numberOfInstallments; }
            set
            {
                if (_numberOfInstallments != value)
                {
                    _numberOfInstallments = value;
                    OnPropertyChanged();
                }
            }
        }

        public double InstallmentAmount
        {
            get { return _installmentsAmount; }
            set
            {
                if (_installmentsAmount != value)
                {
                    _installmentsAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public long NumberOfPaidInstallments
        {
            get { return _numberOfPaidInstallments; }
            set
            {
                if (_numberOfPaidInstallments != value)
                {
                    _numberOfPaidInstallments = value;
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
