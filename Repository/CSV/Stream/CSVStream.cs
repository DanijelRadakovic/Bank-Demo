using Bank.Repository.CSV.Converter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank.Repository.CSV.Stream
{
    public class CSVStream<E> where E : class
    {
        private readonly string _path;
        private readonly ICSVConverter<E> _converter;

        public CSVStream(string path, ICSVConverter<E> converter)
        {
            _path = path;
            _converter = converter;
        }

        public void SaveAll(IEnumerable<E> accounts)
            => WriteAllLinesToFile(
                 accounts
                 .Select(_converter.ConvertEntityToCSVFormat)
                 .ToList());

        public IEnumerable<E> ReadAll()
            => File.ReadAllLines(_path)
                .Select(_converter.ConvertCSVFormatToEntity)
                .ToList();

        public void AppendToFile(E entity)
           => File.AppendAllText(_path, 
               _converter.ConvertEntityToCSVFormat(entity) + Environment.NewLine);

        private void WriteAllLinesToFile(IEnumerable<string> content)
            => File.WriteAllLines(_path, content.ToArray());
    }
}
