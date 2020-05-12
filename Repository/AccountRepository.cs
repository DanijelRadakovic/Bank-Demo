using Bank.Exception;
using Bank.Model;
using Bank.Model.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank.Repository
{
    public class AccountRepository
    {
        private const string NOT_FOUND_ERROR = "Account with {0}:{1} can not be found!";
        private const string NOT_UNIQUE_ERROR = "Account number {0} is not unique!";

        private string _path;
        private string _delimiter;
        private long _accountNextId;

        public AccountRepository(string path, string delimiter)
        {
            _path = path;
            _delimiter = delimiter;
            InitializeId();
        }

        public IEnumerable<Account> GettAll() => ReadAll();

        public Account Get(long id)
        {
            try
            {
                return ReadAll().SingleOrDefault(account => account.Id == id);
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
            if(IsAccountNumberUnique(account.Number))
            {
                account.Id = GenerateAccountId();
                AppendLineToFile(_path, ConvertEntityToCSVFormat(account));
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
                var accounts = ReadAll().ToList();
                accounts[accounts.FindIndex(acc => acc.Id == account.Id)] = account;
                SaveAll(accounts);
            }
            catch (ArgumentException)
            {
                ThrowEntityNotFoundException("id", account.Id);
            }

        }

        public void Delete(Account account)
        {
            var accounts = ReadAll().ToList();
            var accountToRemove = accounts.SingleOrDefault(acc => acc.Id == account.Id);
            if (accountToRemove != null)
            {
                accounts.Remove(accountToRemove);
                SaveAll(accounts);
            }
            else
            {
                ThrowEntityNotFoundException("id", account.Id);
            }
        }

        private long GenerateAccountId() => _accountNextId++;

        private void InitializeId() => _accountNextId = GetMaxId(ReadAll());

        private long GetMaxId(IEnumerable<Account> acounts)
            => acounts.Count() == 0 ? 0 : acounts.Max(acc => acc.Id);

        private bool IsAccountNumberUnique(AccountNumber accountNumber)
           => GetAccountByAccountName(accountNumber) == null;

        private Account GetAccountByAccountName(AccountNumber accountNumber)
            => ReadAll().SingleOrDefault(account => account.Number.Equals(accountNumber));

        private void ThrowEntityNotFoundException(string key, object value)
            => throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, key, value));

        private string ConvertEntityToCSVFormat(Account account)
           => string.Join(_delimiter,
               account.Id,
               account.Number,
               account.Balance);

        private Account ConvertCSVFormatToEntity(string acountCSVFormat)
        {
            string[] tokens = acountCSVFormat.Split(_delimiter.ToCharArray());
            return new Account(
                long.Parse(tokens[0]),
                new AccountNumber(tokens[1]),
                double.Parse(tokens[2]));
        }

        private void SaveAll(IEnumerable<Account> accounts)
            => WriteAllLinesToFile(_path,
                 accounts
                 .Select(ConvertEntityToCSVFormat)
                 .ToList());

        private IEnumerable<Account> ReadAll()
            => File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToEntity)
                .ToList();

        private void AppendLineToFile(string path, string line)
           => File.AppendAllText(path, line + Environment.NewLine);

        private void WriteAllLinesToFile(string path, IEnumerable<string> content)
            => File.WriteAllLines(path, content.ToArray());
    }
}
