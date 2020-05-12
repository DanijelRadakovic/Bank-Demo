using Bank.Model;
using Bank.Service;
using System.Collections.Generic;

namespace Bank.Controller
{
    public class ClientController
    {
        private readonly ClientService service;

        public ClientController(ClientService service)
        {
            this.service = service;
        }

        public IEnumerable<Client> GetAll() => service.GetAll();

        public Client Get(long id) => service.Get(id);

        public Client Create(Client client) => service.Create(client);

        public void Update(Client client) => service.Update(client);

        public void Delete(Client client) => service.Delete(client);
    }
}
