using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.View.Converter
{
    class AbstractConverter
    {
        protected static IList<V> ConvertEntityListToViewList<E, V>(IList<E> entities, Func<E, V> convert)
            => entities
            .Select(convert)
            .ToList();

        protected static IList<E> ConvertViewListToEntityList<E, V>(IList<V> views, Func<V, E> convert)
            => views
            .Select(convert)
            .ToList();
    }
}
