using Bank.Model;
using Bank.Repository;
using System.Collections.Generic;

namespace Bank.Service
{
    public class AccountService : IService<Account,long>
    {
        private readonly IRepository<Account, long> _repository;

        public AccountService(IRepository<Account, long> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Account> GetAll() => _repository.GetAll();

        public Account Get(long id) => _repository.Get(id);

        public Account Create(Account account) => _repository.Create(account);

        public void Update(Account account) => _repository.Update(account);

        public void Delete(Account account) => _repository.Delete(account);
    }
}
