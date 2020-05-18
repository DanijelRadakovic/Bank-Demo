using Bank.Model;
using Bank.Repository.Abstract;
using System.Collections.Generic;

namespace Bank.Service
{
    public class AccountService : IService<Account, long>
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
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
