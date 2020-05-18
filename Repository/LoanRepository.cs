using Bank.Model;
using Bank.Repository.Abstract;
using Bank.Repository.CSV;
using Bank.Repository.CSV.Stream;
using Bank.Repository.Sequencer;

namespace Bank.Repository
{
    public class LoanRepository : CSVRepository<Loan, long>, ILoanRepository
    {
        private const string ENTITY_NAME = "Loan";

        public LoanRepository(ICSVStream<Loan> stream, ISequencer<long> sequencer)
            : base(ENTITY_NAME, stream, sequencer)
        {
        }
    }
}
