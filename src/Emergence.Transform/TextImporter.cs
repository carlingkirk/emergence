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
        private string _filename { get; set; }
        private CsvConfiguration _configuration { get; set; }

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
            using (FileStream fileStream = File.Open(_filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream stream = new BufferedStream(fileStream))
            using (StreamReader streamReader = new StreamReader(stream))
            using (CsvReader reader = new CsvReader(streamReader, _configuration))
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
