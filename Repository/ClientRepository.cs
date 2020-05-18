using Bank.Model;
using Bank.Repository.Abstract;
using Bank.Repository.CSV;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;

namespace Bank.Repository
{
    public class ClientRepository : CSVRepository<Client, long>, IClientRepository
    {
        private const string ENTITY_NAME = "Client";

        public ClientRepository(ICSVStream<Client> stream, ISequencer<long> sequencer)
            : base(ENTITY_NAME, stream, sequencer)
        {
        }
    }
}
