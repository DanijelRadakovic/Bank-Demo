using Bank.Exception;
using Bank.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank.Repository
{
    public class ClientRepository
    {
        private const string NOT_FOUND_ERROR = "Client with {0}:{1} can not be found!";

        private readonly string _path;
        private readonly string _delimiter;
        private readonly string _datetimeFormat;
        private long _clientNextId;

        public ClientRepository(string path, string delimiter, string datetimeFormat)
        {
            _path = path;
            _delimiter = delimiter;
            _datetimeFormat = datetimeFormat;
            InitializeId();
        }

        public IEnumerable<Client> GettAll() => ReadAll();

        public Client Get(long id)
        {
            try
            {
                return ReadAll().SingleOrDefault(client => client.Id == id);
            }
            catch (ArgumentException)
            {
                throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, "id", id));
            }

        }

        public Client Create(Client client)
        {
            client.Id = GenerateClientId();
            AppendLineToFile(_path, ConvertEntityToCSVFormat(client));
            return client;
        }

        public void Update(Client client)
        {
            try
            {
                var clients = ReadAll().ToList();
                clients[clients.FindIndex(clt => clt.Id == client.Id)] = client;
                SaveAll(clients);
            }
            catch (ArgumentException)
            {
                ThrowEntityNotFoundException("id", client.Id);
            }
        }

        public void Delete(Client client)
        {
            var clients = ReadAll().ToList();
            var clientToRemove = clients.SingleOrDefault(acc => acc.Id == client.Id);
            if (clientToRemove != null)
            {
                clients.Remove(clientToRemove);
                SaveAll(clients);
            }
            else
            {
                ThrowEntityNotFoundException("id", client.Id);
            }
        }

        private void InitializeId() => _clientNextId = GetMaxId(ReadAll());

        private long GetMaxId(IEnumerable<Client> clients)
            => clients.Count() == 0 ? 0 : clients.Max(clt => clt.Id);

        private long GenerateClientId() => _clientNextId++;

        private void ThrowEntityNotFoundException(string key, object value)
           => throw new EntityNotFoundException(string.Format(NOT_FOUND_ERROR, key, value));

        private string ConvertEntityToCSVFormat(Client client)
           => string.Join(_delimiter,
               client.Id,
               client.FirstName,
               client.LastName,
               client.DateOfBirth.ToString(_datetimeFormat),
               client.Account.Id);

        private Client ConvertCSVFormatToEntity(string clientCSVFormat)
        {
            string[] tokens = clientCSVFormat.Split(_delimiter.ToCharArray());
            return new Client(
                long.Parse(tokens[0]),
                tokens[1], tokens[2],
                DateTime.Parse(tokens[3]),
                new Account(long.Parse(tokens[4])));
        }

        private IEnumerable<Client> ReadAll()
            => File.ReadAllLines(_path)
                .Select(ConvertCSVFormatToEntity)
                .ToList();

        private void SaveAll(IEnumerable<Client> clients)
            => WriteAllLinesToFile(_path,
                 clients
                 .Select(ConvertEntityToCSVFormat)
                 .ToList());

        private void AppendLineToFile(string path, string line)
           => File.AppendAllText(path, line + Environment.NewLine);

        private void WriteAllLinesToFile(string path, IEnumerable<string> content)
            => File.WriteAllLines(path, content.ToArray());
    }
}
