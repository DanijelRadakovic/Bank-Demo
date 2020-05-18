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
    public class ClientRepository : CSVRepository<Client, long>, 
        IClientRepository,
        IEagerCSVRepository<Client, long>
    {
        private const string ENTITY_NAME = "Client";
        private readonly IEagerCSVRepository<Account, long> _accountRepository;

        public ClientRepository(ICSVStream<Client> stream,
            ISequencer<long> sequencer,
            IEagerCSVRepository<Account, long> accountRepository)
            : base(ENTITY_NAME, stream, sequencer)
        {
            _accountRepository = accountRepository;
        }

        public new IEnumerable<Client> Find(Func<Client, bool> predicate)
            => GetAllEager().Where(predicate);

        public IEnumerable<Client> GetAllEager()
        {
            var accounts = _accountRepository.GetAllEager();
            var clients = GetAll();
            BindAccountsWithClients(accounts, clients);
            return clients;
        }

        public Client GetEager(long id)
        {
            var client = Get(id);
            client.Account = _accountRepository.GetEager(client.Account.Id);
            return client;
        }

        private void BindAccountsWithClients(IEnumerable<Account> accounts, IEnumerable<Client> clients)
           => clients
           .ToList()
           .ForEach(client => client.Account = GetAccountById(accounts, client.Id));

        private Account GetAccountById(IEnumerable<Account> accounts, long id)
            => accounts.SingleOrDefault(account => account.Id == id);
    }
}
