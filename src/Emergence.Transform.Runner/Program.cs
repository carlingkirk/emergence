using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Emergence.Data.External.USDA;
using Emergence.Data.Repository;
using Emergence.Data.Shared.Stores;
using Emergence.Service;
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
            var dbContext = new EmergenceDbContext();
            var lifeformService = new LifeformService(new Repository<Lifeform>(dbContext));
            var originService = new OriginService(new Repository<Origin>(dbContext));
            var plantInfoService = new PlantInfoService(new Repository<PlantInfo>(dbContext));

            var transformer = new USDATransformer();
            var origin = await originService.GetOriginAsync(transformer.Origin.OriginId);
            if (origin == null)
            {
                origin = await originService.AddOrUpdateOriginAsync(transformer.Origin);
            }

            foreach (var importer in importers)
            {
                if (importer.Type == ImporterType.TextImporter)
                {
                    var dataFile = FileHelpers.GetDatafileName(importer.Filename, dataDirectory);
                    var textImporter = new TextImporter<Checklist>(dataFile, importer.HasHeaders);
                    await foreach (var result in textImporter.Import())
                    {
                        var plantInfo = transformer.Transform(result);
                        var lifeform = await lifeformService.GetLifeformByScientificNameAsync(plantInfo.ScientificName);
                        if (lifeform == null)
                        {
                            lifeform = await lifeformService.AddOrUpdateLifeformAsync(plantInfo.Lifeform);
                        }
                        plantInfo.Lifeform = lifeform;
                        var originResult = await originService.GetOriginAsync(origin.OriginId, plantInfo.Origin.ExternalId, plantInfo.Origin.AltExternalId);
                        if (originResult == null)
                        {
                            originResult = await originService.AddOrUpdateOriginAsync(plantInfo.Origin);
                        }

                        plantInfo.Origin = originResult;

                        var plantInfoResult = plantInfoService.GetPlantInfoAsync(plantInfo.Origin.OriginId, plantInfo.Taxon.TaxonId);
                        plantInfo = await plantInfoService.AddOrUpdatePlantInfoAsync(plantInfo);

                        Console.WriteLine(plantInfo.ScientificName);
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
