using Bank.Model;
using System.Collections.Generic;

namespace Bank.Repository.Abstract
{
    public interface ILoanRepository : IRepository<Loan, long>
    {
        IEnumerable<Loan> GetByInterestRateGreaterThanAndClientBalanceGreaterThan(
            double interestRate, double clientBalance);
        IEnumerable<Loan> GetByBaseOrderByAscWithPagination(int pageIndex, int pageSize = 10);
    }
}
