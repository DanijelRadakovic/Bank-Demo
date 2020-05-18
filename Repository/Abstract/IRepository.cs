using System;
using System.Collections.Generic;

namespace Bank.Repository.Abstract
{
    public interface IRepository<E, ID>
        where E : IIdentifiable<ID>
        where ID : IComparable
    {
        E Get(ID id);
        IEnumerable<E> GetAll();
        E Create(E entity);
        void Update(E entity);
        void Delete(E entity);
        IEnumerable<E> Find(Func<E, bool> predicate);
    }
}
