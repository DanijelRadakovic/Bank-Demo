using Bank.Model;
using Bank.Repository;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Service
{
    class ClientService
    {
        private readonly ClientRepository clientRepository;
        private readonly AccountService accountService;

        public IEnumerable<Client> GetAll()
        {
            var accounts = accountService.GetAll();
            var clients = clientRepository.GettAll();
            BindAccountsWithClients(accounts, clients);
            return clients;
        }

        public Client Get(long id)
        {
            var client = clientRepository.Get(id);
            client.Account = accountService.Get(client.Account.Id);
            return client;
        }

        public Client Create(Client client)
        {
            var account = accountService.Create(client.Account);
            var newClient = clientRepository.Create(client);
            newClient.Account = account;
            return newClient;
        }

        public void Update(Client client)
        {
            accountService.Update(client.Account);
            clientRepository.Update(client);
        }

        public void Delete(Client client)
        {
            accountService.Delete(client.Account);
            clientRepository.Delete(client);
        }

        private void BindAccountsWithClients(IEnumerable<Account> accounts, IEnumerable<Client> clients)
            => clients
            .ToList()
            .ForEach(client => client.Account = GetAccountById(accounts, client.Id));

        private Account GetAccountById(IEnumerable<Account> accounts, long id)
            => accounts.SingleOrDefault(account => account.Id == id);
    }
}
