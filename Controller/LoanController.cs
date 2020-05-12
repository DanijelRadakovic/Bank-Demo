using Bank.Model;
using Bank.Service;
using System.Collections.Generic;

namespace Bank.Controller
{
    public class LoanController
    {
        private readonly LoanService service;

        public LoanController(LoanService service)
        {
            this.service = service;
        }

        public IEnumerable<Loan> GetAll() => service.GetAll();

        public Loan Get(long id) => service.Get(id);

        public Loan Create(Loan loan) => service.Create(loan);

        public void Update(Loan loan) => service.Update(loan);

        public void Delete(Loan loan) => service.Delete(loan);
    }
}
