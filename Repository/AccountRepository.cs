using Bank.Exception;
using Bank.Model;
using Bank.Model.Util;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Repository
{
    public class AccountRepository : IRepository<Account, long>
    {
        private const string NOT_FOUND_ERROR = "Account with {0}:{1} can not be found!";
        private const string NOT_UNIQUE_ERROR = "Account number {0} is not unique!";

        private readonly ICSVStream<Account> _stream;
        private readonly ISequencer<long> _sequencer;

        public AccountRepository(CSVStream<Account> stream, ISequencer<long> sequencer)
        {
            _stream = stream;
            _sequencer = sequencer;
            _sequencer.Initialize(GetMaxId(_stream.ReadAll()));
        }

        public IEnumerable<Account> GetAll() => _stream.ReadAll();

        public Account Get(long id)
        {
            try
            {
                return _stream.ReadAll().SingleOrDefault(account => account.Id == id);
            }
            catch (ArgumentException)
            {
                throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, "id", id));
            }

        }

        public Account GetByAccountNumber(AccountNumber accountNumber)
        {
            var account = GetAccountByAccountName(accountNumber);
            if (account != null)
            {
                return account;
            }
            else
            {
                throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, "account number", accountNumber));
            }
        }

        public Account Create(Account account)
        {
            if (IsAccountNumberUnique(account.Number))
            {
                account.Id = GenerateAccountId();
                _stream.AppendToFile(account);
                return account;
            }
            else
            {
                throw new NotUniqueException(string.Format(NOT_UNIQUE_ERROR, account.Number));
            }

        }

        public void Update(Account account)
        {
            try
            {
                var accounts = _stream.ReadAll().ToList();
                accounts[accounts.FindIndex(acc => acc.Id == account.Id)] = account;
                _stream.SaveAll(accounts);
            }
            catch (ArgumentException)
            {
                ThrowEntityNotFoundException("id", account.Id);
            }

        }

        public void Delete(Account account)
        {
            var accounts = _stream.ReadAll().ToList();
            var accountToRemove = accounts.SingleOrDefault(acc => acc.Id == account.Id);
            if (accountToRemove != null)
            {
                accounts.Remove(accountToRemove);
                _stream.SaveAll(accounts);
            }
            else
            {
                ThrowEntityNotFoundException("id", account.Id);
            }
        }

        private long GenerateAccountId() => _sequencer.GenerateId();

        private long GetMaxId(IEnumerable<Account> acounts)
            => acounts.Count() == 0 ? 0 : acounts.Max(acc => acc.Id);

        private bool IsAccountNumberUnique(AccountNumber accountNumber)
           => GetAccountByAccountName(accountNumber) == null;

        private Account GetAccountByAccountName(AccountNumber accountNumber)
            => _stream.ReadAll().SingleOrDefault(account => account.Number.Equals(accountNumber));

        private void ThrowEntityNotFoundException(string key, object value)
            => throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, key, value));
    }
}
