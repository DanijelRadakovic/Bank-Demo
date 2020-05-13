using Bank.Model;
using Bank.Service;
using System.Collections.Generic;

namespace Bank.Controller
{
    public class TransactionController
    {
        private readonly TransactionService service;

        public TransactionController(TransactionService service)
        {
            this.service = service;
        }

        public IEnumerable<Transaction> GetAll() => service.GetAll();

        public Transaction Get(long id) => service.Get(id);

        public Transaction Create(Transaction transaction) => service.Create(transaction);

        public void Update(Transaction transaction) => service.Update(transaction);

        public void Delete(Transaction transaction) => service.Delete(transaction);
    }
}
