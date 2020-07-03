using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Emergence.Transform
{
    public interface ITextImporter<T> : IImporter<T>
    {
    }

    public class TextImporter<T> : ITextImporter<T>
    {
        private readonly string _filename;
        private readonly CsvConfiguration _configuration;

        public TextImporter(string filename, bool hasHeaders)
        {
            _filename = filename;
            _configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = hasHeaders
            };
        }

        public async IAsyncEnumerable<T> Import()
        {
            using (var fileStream = File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var stream = new BufferedStream(fileStream))
            using (var streamReader = new StreamReader(stream))
            using (var reader = new CsvReader(streamReader, _configuration))
            {
                var records = reader.GetRecordsAsync<T>().GetAsyncEnumerator();

                while (await records.MoveNextAsync())
                {
                    yield return records.Current;
                }
            }
        }
    }
}
