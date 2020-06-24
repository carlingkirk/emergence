using Emergence.Data.External.USDA;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Emergence.Transform.Runner
{
    class Program
    {
        static async Task Main(string[] args)
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
                    await foreach(var result in textImporter.Import())
                    {
                        Console.WriteLine(result.ScientificNameWithAuthor);
                    }
                }
            }
            Console.ReadKey();
        }

        static IEnumerable<ImporterConfiguration> LoadImporters(IConfigurationRoot configuration)
        {
            var importers = configuration.GetSection("importers").GetChildren();
            foreach(var importer in importers)
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

        static IConfigurationRoot LoadConfiguration()
        {
            return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
    }
}
