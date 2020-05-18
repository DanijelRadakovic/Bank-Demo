using Bank.Model;
using Bank.Repository.Abstract;
using Bank.Repository.CSV;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Repository
{
    public class TransactionRepository : CSVRepository<Transaction, long>, 
        ITransactionRepository,
        IEagerCSVRepository<Transaction, long>
    {
        private const string ENTITY_NAME = "Transaction";
        private readonly IEagerCSVRepository<Client, long> _clientRepository;

        public TransactionRepository(ICSVStream<Transaction> stream, 
            ISequencer<long> sequencer,
            IEagerCSVRepository<Client, long> clientRepository)
            : base(ENTITY_NAME, stream, sequencer)
        {
            _clientRepository = clientRepository;
        }

        public new IEnumerable<Transaction> Find(Func<Transaction, bool> predicate)
            => GetAllEager().Where(predicate);

        public IEnumerable<Transaction> GetAllEager()
        {
            var clients = _clientRepository.GetAllEager();
            var transactions = GetAll();
            BindClientsWithTransactions(clients, transactions);
            return transactions;
        }

        public Transaction GetEager(long id)
        {
            var transaction = Get(id);
            transaction.Payer = _clientRepository.GetEager(transaction.Payer.Id);
            transaction.Receiver = _clientRepository.GetEager(transaction.Receiver.Id);
            return transaction;
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
