using Bank.Model;
using Bank.Service;
using System.Collections.Generic;

namespace Bank.Controller
{
    public class ClientController : IController<Client, long>
    {
        private readonly IService<Client, long> _service;

        public ClientController(IService<Client, long> service)
        {
            _service = service;
        }

        public IEnumerable<Client> GetAll() => _service.GetAll();

        public Client Get(long id) => _service.Get(id);

        public Client Create(Client client) => _service.Create(client);

        public void Update(Client client) => _service.Update(client);

        public void Delete(Client client) => _service.Delete(client);
    }
}
