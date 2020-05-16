using Bank.Exception;
using Bank.Model;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Repository
{
    public class ClientRepository
    {
        private const string NOT_FOUND_ERROR = "Client with {0}:{1} can not be found!";

        private readonly CSVStream<Client> _stream;
        private readonly ISequencer<long> _sequencer;

        public ClientRepository(CSVStream<Client> stream, ISequencer<long> sequencer)
        {
            _stream = stream;
            _sequencer = sequencer;
            _sequencer.Initialize(GetMaxId(_stream.ReadAll()));
        }

        public IEnumerable<Client> GettAll() => _stream.ReadAll();

        public Client Get(long id)
        {
            try
            {
                return _stream.ReadAll().SingleOrDefault(client => client.Id == id);
            }
            catch (ArgumentException)
            {
                throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, "id", id));
            }

        }

        public Client Create(Client client)
        {
            client.Id = GenerateClientId();
            _stream.AppendToFile(client);
            return client;
        }

        public void Update(Client client)
        {
            try
            {
                var clients = _stream.ReadAll().ToList();
                clients[clients.FindIndex(clt => clt.Id == client.Id)] = client;
                _stream.SaveAll(clients);
            }
            catch (ArgumentException)
            {
                ThrowEntityNotFoundException("id", client.Id);
            }
        }

        public void Delete(Client client)
        {
            var clients = _stream.ReadAll().ToList();
            var clientToRemove = clients.SingleOrDefault(acc => acc.Id == client.Id);
            if (clientToRemove != null)
            {
                clients.Remove(clientToRemove);
                _stream.SaveAll(clients);
            }
            else
            {
                ThrowEntityNotFoundException("id", client.Id);
            }
        }

        private long GetMaxId(IEnumerable<Client> clients)
            => clients.Count() == 0 ? 0 : clients.Max(clt => clt.Id);

        private long GenerateClientId() => _sequencer.GenerateId();

        private void ThrowEntityNotFoundException(string key, object value)
           => throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, key, value));
    }
}
