using Bank.Exception;
using Bank.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank.Repository
{
    class AccountRepository
    {
        private string _path;
        private string _delimiter;
        private long _accountNextId;

        public IEnumerable<Account> GettAll() => ReadAccountsFromFile();

        public Account Create(Account account)
        {
            account.Id = GenerateAccountId();
            AppendLineToFile(_path, ConvertEntityToCSVFormat(account));
            return account;
        }

        
        public void Update(Account account)
        {
            var accounts = ReadAccountsFromFile().ToList();
            accounts[accounts.FindIndex(acc => acc.Id == account.Id)] = account;
            SaveAll(accounts);
        }

        public void Delete(Account account)
        {
            var accounts = ReadAccountsFromFile().ToList();
            var accountToRemove = accounts.SingleOrDefault(acc => acc.Id == account.Id);
            if (accountToRemove != null)
            {
                accounts.Remove(accountToRemove);
                SaveAll(accounts);
            }
            else
            {
                throw new EntityNotFoundException($"Client with id: {account.Id} can not be found!");
            }

        }

        private IEnumerable<Account> ReadAccountsFromFile()
            => File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToEntity)
                .ToList();

        private string ConvertEntityToCSVFormat(Account account)
            => string.Join(_delimiter,
                account.Id,
                account.Number,
                account.Balance);

        private long GenerateAccountId() => _accountNextId++;

        private Account ConvertCSVFormatToEntity(string acountCSVFormat)
        {
            string[] tokens = acountCSVFormat.Split(_delimiter.ToCharArray());
            return new Account(
                long.Parse(tokens[0]), 
                new Model.Util.AccountNumber(tokens[1]), 
                double.Parse(tokens[2]));
        }

        private void SaveAll(IEnumerable<Account> accounts)
            => WriteAllLinesToFile(_path,
                 accounts
                 .Select(ConvertEntityToCSVFormat)
                 .ToList());

        private void AppendLineToFile(string path, string line)
           => File.AppendAllText(path, line + Environment.NewLine);

        private void WriteAllLinesToFile(string path, IEnumerable<string> content)
            => File.WriteAllLines(path, content.ToArray());
    }
}
