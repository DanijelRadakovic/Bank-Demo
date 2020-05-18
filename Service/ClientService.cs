using Bank.Model;
using Bank.Repository;
using Bank.Repository.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Service
{
    public class ClientService : IService<Client, long>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IService<Account, long> _accountService;

        public ClientService(IClientRepository clientRepository,
            IService<Account, long> accountService)
        {
            _clientRepository = clientRepository;
            _accountService = accountService;
        }

        public IEnumerable<Client> GetAll()
        {
            var accounts = _accountService.GetAll();
            var clients = _clientRepository.GetAll();
            BindAccountsWithClients(accounts, clients);
            return clients;
        }

        public Client Get(long id)
        {
            var client = _clientRepository.Get(id);
            client.Account = _accountService.Get(client.Account.Id);
            return client;
        }

        public Client Create(Client client)
        {
            var account = _accountService.Create(client.Account);
            var newClient = _clientRepository.Create(client);
            newClient.Account = account;
            return newClient;
        }

        public void Update(Client client)
        {
            _accountService.Update(client.Account);
            _clientRepository.Update(client);
        }

        public void Delete(Client client)
        {
            _accountService.Delete(client.Account);
            _clientRepository.Delete(client);
        }

        private void BindAccountsWithClients(IEnumerable<Account> accounts, IEnumerable<Client> clients)
            => clients
            .ToList()
            .ForEach(client => client.Account = GetAccountById(accounts, client.Id));

        private Account GetAccountById(IEnumerable<Account> accounts, long id)
            => accounts.SingleOrDefault(account => account.Id == id);
    }
}
