using Bank.Exception;
using Bank.Model;
using Bank.Model.Util;
using Bank.Repository.Abstract;
using Bank.Repository.CSV;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Repository
{
    public class AccountRepository : CSVRepository<Account, long>, 
        IAccountRepository,
        IEagerCSVRepository<Account, long>
    {
        private const string ENTITY_NAME = "Account";
        private const string NOT_UNIQUE_ERROR = "Account number {0} is not unique!";


        public AccountRepository(ICSVStream<Account> stream, ISequencer<long> sequencer)
            : base(ENTITY_NAME, stream, sequencer)
        {
        }

        public new Account Create(Account account)
        {
            if (IsAccountNumberUnique(account.Number))
                return base.Create(account);
            else
                throw new NotUniqueException(string.Format(NOT_UNIQUE_ERROR, account.Number));
        }

        private bool IsAccountNumberUnique(AccountNumber accountNumber)
           => GetAccountByAccountName(accountNumber) == null;

        private Account GetAccountByAccountName(AccountNumber accountNumber)
            => _stream.ReadAll().SingleOrDefault(account => account.Number.Equals(accountNumber));

        public Account GetEager(long id) => Get(id);

        public IEnumerable<Account> GetAllEager() => GetAll();
    }
}
