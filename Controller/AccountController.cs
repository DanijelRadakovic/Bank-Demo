using Bank.Model;
using Bank.Service;
using System.Collections.Generic;

namespace Bank.Controller
{
    public class AccountController
    {
        private readonly AccountService service;

        public AccountController(AccountService service)
        {
            this.service = service;
        }

        public IEnumerable<Account> GetAll() => service.GetAll();

        public Account Get(long id) => service.Get(id);

        public Account Create(Account account) => service.Create(account);

        public void Update(Account account) => service.Update(account);

        public void Delete(Account account) => service.Delete(account);
    }
}
