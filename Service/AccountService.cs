using Bank.Model;
using Bank.Repository;
using System.Collections.Generic;

namespace Bank.Service
{
    public class AccountService
    {
        private readonly AccountRepository repository;

        public AccountService(AccountRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Account> GetAll() => repository.GettAll();

        public Account Get(long id) => repository.Get(id);

        public Account Create(Account account) => repository.Create(account);

        public void Update(Account account) => repository.Update(account);

        public void Delete(Account account) => repository.Delete(account);
    }
}
