﻿using System.Collections.Generic;

namespace Bank.Repository.CSV.Stream
{
    public interface ICSVStream<E>
    {
        void SaveAll(IEnumerable<E> entities);
        IEnumerable<E> ReadAll();
        void AppendToFile(E entity);
    }
}
