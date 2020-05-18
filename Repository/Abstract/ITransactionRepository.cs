using Bank.Model;

namespace Bank.Repository.Abstract
{
    public interface ITransactionRepository : IRepository<Transaction, long>
    {
    }
}
