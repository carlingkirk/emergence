using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Emergence.Data.External.USDA;
using Microsoft.Extensions.Configuration;

namespace Emergence.Transform.Runner
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var importers = LoadImporters(configuration);
            var dataDirectory = configuration["dataDirectory"];
            foreach (var importer in importers)
            {
                if (importer.Type == ImporterType.TextImporter)
                {
                    var dataFile = FileHelpers.GetDatafileName(importer.Filename, dataDirectory);
                    var textImporter = new TextImporter<Checklist>(dataFile, importer.HasHeaders);
                    await foreach (var result in textImporter.Import())
                    {
                        Console.WriteLine(result.ScientificNameWithAuthor);
                    }
                }
            }
            Console.ReadKey();
        }

        public static IEnumerable<ImporterConfiguration> LoadImporters(IConfigurationRoot configuration)
        {
            var importers = configuration.GetSection("importers").GetChildren();
            foreach (var importer in importers)
            {
                yield return new ImporterConfiguration
                {
                    Name = importer["name"],
                    Type = Enum.Parse<ImporterType>(importer["type"]),
                    Filename = importer["filename"],
                    HasHeaders = bool.Parse(importer["hasHeaders"])
                };
            }
        }

        public static IConfigurationRoot LoadConfiguration() => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    }
}
