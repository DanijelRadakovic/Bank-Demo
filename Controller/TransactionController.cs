using Bank.Model;
using Bank.Service;
using System.Collections.Generic;

namespace Bank.Controller
{
    public class TransactionController : IController<Transaction, long>
    {
        private readonly IService<Transaction, long> _service;

        public TransactionController(IService<Transaction, long> service)
        {
            _service = service;
        }

        public IEnumerable<Transaction> GetAll() => _service.GetAll();

        public Transaction Get(long id) => _service.Get(id);

        public Transaction Create(Transaction transaction) => _service.Create(transaction);

        public void Update(Transaction transaction) => _service.Update(transaction);

        public void Delete(Transaction transaction) => _service.Delete(transaction);
    }
}
