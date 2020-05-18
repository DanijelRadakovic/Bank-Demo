using Bank.Model;
using Bank.Repository.Abstract;
using Bank.Repository.CSV;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;

namespace Bank.Repository
{
    public class TransactionRepository : CSVRepository<Transaction, long>, ITransactionRepository
    {
        private const string ENTITY_NAME = "Loan";

        public TransactionRepository(ICSVStream<Transaction> stream, ISequencer<long> sequencer)
            : base(ENTITY_NAME, stream, sequencer)
        {
        }
    }
}
