using Bank.Model;
using Bank.Service;
using System.Collections.Generic;

namespace Bank.Controller
{
    public class LoanController : IController<Loan, long>
    {
        private readonly IService<Loan, long> _service;

        public LoanController(IService<Loan, long> service)
        {
            _service = service;
        }

        public IEnumerable<Loan> GetAll() => _service.GetAll();

        public Loan Get(long id) => _service.Get(id);

        public Loan Create(Loan loan) => _service.Create(loan);

        public void Update(Loan loan) => _service.Update(loan);

        public void Delete(Loan loan) => _service.Delete(loan);
    }
}
