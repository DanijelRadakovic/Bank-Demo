using System.Collections.Generic;

namespace Bank.Controller
{
    public interface IController<E, ID> where E : class
    {
        IEnumerable<E> GetAll();
        E Get(ID id);
        E Create(E entity);
        void Update(E entity);
        void Delete(E entity);
    }
}
