using Bank.Model;
using Bank.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Service
{
    public class TransactionService
    {
        private readonly TransactionRepository transactionRepository;
        private readonly ClientService clientService;

        public TransactionService(TransactionRepository transactionRepository, ClientService clientService)
        {
            this.transactionRepository = transactionRepository;
            this.clientService = clientService;
        }

        public IEnumerable<Transaction> GetAll()
        {
            var clients = clientService.GetAll();
            var transactions = transactionRepository.GettAll();
            BindClientsWithTransactions(clients, transactions);
            return transactions;
        }

        public Transaction Get(long id)
        {
            var transaction = transactionRepository.Get(id);
            transaction.Payer = clientService.Get(transaction.Payer.Id);
            transaction.Receiver = clientService.Get(transaction.Receiver.Id);
            return transaction;
        }

        public Transaction Create(Transaction transaction)
        {
            SetMissingValues(transaction);
            ExecuteTransaction(transaction);

            clientService.Update(transaction.Payer);
            clientService.Update(transaction.Receiver);
            Transaction newTransaction = transactionRepository.Create(transaction);
            newTransaction.Payer = transaction.Payer;
            newTransaction.Receiver = transaction.Receiver;

            return newTransaction;
        }

        public void Update(Transaction transaction)
        {
            clientService.Update(transaction.Payer);
            clientService.Update(transaction.Receiver);
            transactionRepository.Update(transaction);
        }

        public void Delete(Transaction transaction)
        {
            clientService.Delete(transaction.Payer);
            clientService.Delete(transaction.Receiver);
            transactionRepository.Delete(transaction);
        }



        private void SetMissingValues(Transaction transaction)
        {
            transaction.Date = DateTime.Now;
            transaction.Payer = clientService.Get(transaction.Payer.Id);
            transaction.Receiver = clientService.Get(transaction.Receiver.Id);
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
