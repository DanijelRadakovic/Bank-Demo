using Bank.Model;
using Bank.Service;
using System.Collections.Generic;

namespace Bank.Controller
{
    public class AccountController : IController<Account, long>
    {
        private readonly IService<Account, long> _service;

        public AccountController(IService<Account, long> service)
        {
            _service = service;
        }

        public IEnumerable<Account> GetAll() => _service.GetAll();

        public Account Get(long id) => _service.Get(id);

        public Account Create(Account account) => _service.Create(account);

        public void Update(Account account) => _service.Update(account);

        public void Delete(Account account) => _service.Delete(account);
    }
}
