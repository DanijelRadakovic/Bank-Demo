using Bank.Repository.Abstract;
using System;
using System.Collections.Generic;

namespace Bank.Repository.CSV
{
    public interface IEagerCSVRepository<E, ID>
        where E : IIdentifiable<ID>
        where ID : IComparable
    {
        E GetEager(ID id);
        IEnumerable<E> GetAllEager();
    }
}
