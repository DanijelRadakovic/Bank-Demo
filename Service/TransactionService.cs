using Bank.Model;
using Bank.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Service
{
    public class TransactionService : IService<Transaction, long>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IService<Client, long> _clientService;

        public TransactionService(ITransactionRepository transactionRepository,
            IService<Client, long> clientService)
        {
            _transactionRepository = transactionRepository;
            _clientService = clientService;
        }

        public IEnumerable<Transaction> GetAll()
        {
            var clients = _clientService.GetAll();
            var transactions = _transactionRepository.GetAll();
            BindClientsWithTransactions(clients, transactions);
            return transactions;
        }

        public Transaction Get(long id)
        {
            var transaction = _transactionRepository.Get(id);
            transaction.Payer = _clientService.Get(transaction.Payer.Id);
            transaction.Receiver = _clientService.Get(transaction.Receiver.Id);
            return transaction;
        }

        public Transaction Create(Transaction transaction)
        {
            SetMissingValues(transaction);
            ExecuteTransaction(transaction);

            _clientService.Update(transaction.Payer);
            _clientService.Update(transaction.Receiver);
            Transaction newTransaction = _transactionRepository.Create(transaction);
            newTransaction.Payer = transaction.Payer;
            newTransaction.Receiver = transaction.Receiver;

            return newTransaction;
        }

        public void Update(Transaction transaction)
        {
            _clientService.Update(transaction.Payer);
            _clientService.Update(transaction.Receiver);
            _transactionRepository.Update(transaction);
        }

        public void Delete(Transaction transaction)
        {
            _clientService.Delete(transaction.Payer);
            _clientService.Delete(transaction.Receiver);
            _transactionRepository.Delete(transaction);
        }



        private void SetMissingValues(Transaction transaction)
        {
            transaction.Date = DateTime.Now;
            transaction.Payer = _clientService.Get(transaction.Payer.Id);
            transaction.Receiver = _clientService.Get(transaction.Receiver.Id);
        }

        private void ExecuteTransaction(Transaction transaction)
        {
            transaction.Payer.Account.Balance -= transaction.Amount.Value;
            transaction.Receiver.Account.Balance += transaction.Amount.Value;
        }

        private void BindClientsWithTransactions(IEnumerable<Client> clients, IEnumerable<Transaction> transactions)
            => transactions.ToList().ForEach(transaction =>
            {
                transaction.Payer = FindClientById(clients, transaction.Payer.Id);
                transaction.Receiver = FindClientById(clients, transaction.Receiver.Id);
            });

        private Client FindClientById(IEnumerable<Client> clients, long id)
           => clients.SingleOrDefault(client => client.Id == id);
    }
}
