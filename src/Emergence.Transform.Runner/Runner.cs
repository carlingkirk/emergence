using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.External.ITIS;
using Emergence.Data.External.NatureServe;
using Emergence.Data.External.USDA;
using Emergence.Data.Shared.Models;
using Emergence.Transform.NatureServe;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Emergence.Transform.Runner
{
    public class Runner
    {
        private readonly ILogger<Runner> _logger;
        private readonly IConfiguration Configuration;
        private readonly IImportTransformOrchestrator _importTransformOrchestrator;

        public Runner(ILogger<Runner> logger, IConfiguration configuration, IImportTransformOrchestrator importTransformOrchestrator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Configuration = configuration;
            _importTransformOrchestrator = importTransformOrchestrator;
        }

        public async Task Run(string[] args)
        {
            var importers = LoadImporters(Configuration);
            string dataDirectory;
            if (args.Length != 0)
            {
                dataDirectory = args[0];
            }
            else
            {
                dataDirectory = Configuration["dataDirectory"];
            }

            foreach (var importer in importers.Where(i => i.IsActive))
            {
                if (importer.Type == ImporterType.TextImporter && importer.ImportModel == "TaxonomicUnit")
                {
                    var processor = _importTransformOrchestrator.GetPlantInfoProcessor;
                    var transformer = new USDATransformer();
                    var startRow = 1;
                    var batchSize = 100;

                    await processor.InitializeOrigin(transformer.Origin);
                    await processor.InitializeLifeforms();
                    await processor.InitializeTaxons();

                    var dataFile = FileHelpers.GetDatafileName(importer.Filename, dataDirectory);
                    var textImporter = new TextImporter<Checklist>(dataFile, importer.HasHeaders);
                    var row = 1;
                    var checklists = new List<Checklist>();
                    await foreach (var result in textImporter.Import())
                    {
                        row++;
                        if (row < startRow)
                        {
                            continue;
                        }
                        else if (row % batchSize != 0)
                        {
                            checklists.Add(result);
                        }
                        else
                        {
                            if (checklists.Any())
                            {
                                await ProcessChecklists(transformer, processor, checklists);

                                checklists.Clear();
                            }
                        }

                        if (checklists.Any())
                        {
                            await ProcessChecklists(transformer, processor, checklists);

                            checklists.Clear();
                        }
                    }
                }
                else if (importer.Type == ImporterType.SqlImporter && importer.ImportModel == "TaxonomicUnit")
                {
                    var processor = _importTransformOrchestrator.GetPlantInfoProcessor;
                    var sqlImporter = new SqlImporter<TaxonomicUnit>(importer.ConnectionString, importer.SqlQuery);
                    var transformer = new ITISPlantInfoTransformer();
                    var startRow = 0;
                    var batchSize = 100;
                    var row = 0;
                    var taxonomicUnits = new List<TaxonomicUnit>();

                    await processor.InitializeOrigin(transformer.Origin);
                    await processor.InitializeLifeforms();
                    await processor.InitializeTaxons();

                    await foreach (var result in sqlImporter.Import())
                    {
                        row++;
                        if (row < startRow)
                        {
                            continue;
                        }
                        else if (row % batchSize != 0)
                        {
                            taxonomicUnits.Add(result);
                        }
                        else
                        {
                            if (taxonomicUnits.Any())
                            {
                                await ProcessTaxonomicUnits(transformer, processor, taxonomicUnits);

                                taxonomicUnits.Clear();
                            }
                        }
                    }

                    if (taxonomicUnits.Any())
                    {
                        await ProcessTaxonomicUnits(transformer, processor, taxonomicUnits);

                        taxonomicUnits.Clear();
                    }
                }
                else if (importer.Type == ImporterType.SqlImporter && importer.ImportModel == "Vernacular")
                {
                    var processor = _importTransformOrchestrator.GetSynonymProcessor;
                    var sqlImporter = new SqlImporter<Vernacular>(importer.ConnectionString, importer.SqlQuery);
                    var transformer = new ITISSynonymTransformer();
                    var startRow = 0;
                    var batchSize = 500;
                    var row = 0;
                    var vernaculars = new List<Vernacular>();

                    await processor.InitializeOrigin(transformer.Origin);
                    await processor.InitializeTaxons();

                    await foreach (var result in sqlImporter.Import())
                    {
                        row++;
                        if (row < startRow)
                        {
#pragma warning disable S3626 // Jump statements should not be redundant
                            continue;
#pragma warning restore S3626 // Jump statements should not be redundant
                        }
                        else if (row % batchSize != 0)
                        {
                            vernaculars.Add(result);
                        }
                        else
                        {
                            await ProcessSynonyms(transformer, processor, vernaculars);
                            vernaculars.Clear();
                        }
                    }
                    if (vernaculars.Any())
                    {
                        await ProcessSynonyms(transformer, processor, vernaculars);
                        vernaculars.Clear();
                    }
                }
                else if (importer.Type == ImporterType.EFImporter && importer.ImportModel == "PlantInfo")
                {
                    var processor = _importTransformOrchestrator.GetElasticPlantInfoProcessor;
                    var response = await processor.ProcessSome();
                    Console.WriteLine($"Successes: {response.Successes} / Failures: {response.Failures}");
                    //var finished = false;
                    //var counter = 100000;
                    //var counterEnd = 1000000;

                    //while (!finished && counter < counterEnd)
                    //{
                    //    var response = await processor.Process(counter, counter + 1000);

                    //    Console.WriteLine($"Successes: {response.Successes} / Failures: {response.Failures}");

                    //    if (response.Failures > 0)
                    //    {
                    //        throw new Exception("Errors! Noooooo!");
                    //    }
                    //    else if (response.Successes == 0)
                    //    {
                    //        finished = true;
                    //    }

                    //    counter += 1000;
                    //}
                }
                else if (importer.Type == ImporterType.EFImporter && importer.ImportModel == "Specimen")
                {
                    var processor = _importTransformOrchestrator.GetElasticSpecimenProcessor;
                    var finished = false;
                    var counter = 0;
                    var counterEnd = 1000000;

                    while (!finished && counter < counterEnd)
                    {
                        var response = await processor.Process(counter, counter + 1000);

                        Console.WriteLine($"Successes: {response.Successes} / Failures: {response.Failures}");

                        if (response.Failures > 0)
                        {
                            throw new Exception("Errors! Noooooo!");
                        }
                        else if (response.Successes == 0)
                        {
                            finished = true;
                        }

                        counter += 1000;
                    }
                }
                else if (importer.Type == ImporterType.JsonImporter && importer.ImportModel == "PlantsFile")
                {

                    var transformer = new NatureServeTransformer();
                    var dataFile = FileHelpers.GetDatafileName(importer.Filename, dataDirectory);
                    var textImporter = new JsonImporter<IEnumerable<Plant>>(dataFile);
                    var plants = textImporter.ImportObjectAsync();
                    var plantInfos = new List<PlantInfo>();

                    List<Taxon> taxons;
                    List<Lifeform> lifeforms;
                    List<Origin> origins;

                    foreach (var plant in plants)
                    {
                        plantInfos.Add(transformer.Transform(plant));
                    }

                    var processor = _importTransformOrchestrator.GetNatureServePlantInfoProcessor();
                    await processor.InitializeOrigin(transformer.Origin);
                    await processor.InitializeLifeforms();
                    await processor.InitializeTaxons();
                    await processor.InitializeOrigins();

                    taxons = processor.Taxons;
                    lifeforms = processor.Lifeforms;
                    origins = processor.Origins;

                    var batchSize = 500;
                    var finished = false;
                    var start = 0;
                    var counter = start;
                    var counterEnd = 40000;

                    plantInfos = plantInfos.OrderBy(p => p.Origin.ExternalId).ToList();

                    while (!finished && counter < counterEnd)
                    {
                        if (counter > start)
                        {
                            processor = _importTransformOrchestrator.GetNatureServePlantInfoProcessor(lifeforms, taxons, origins);
                            await processor.InitializeOrigin(transformer.Origin);
                        }

                        var batch = plantInfos.Skip(counter).Take(batchSize);

                        if (!batch.Any())
                        {
                            finished = true;
                            continue;
                        }

                        var result = await processor.Process(batch);
                        Console.WriteLine($"Processed: {result.Count()} plantInfos - ended on {batch.Last().ScientificName}");
                        counter += batchSize;
                    }
                }
            }
        }

        private async Task ProcessChecklists(USDATransformer transformer, PlantInfoProcessor processor, List<Checklist> checklists)
        {
            var plantInfos = new List<PlantInfo>();
            foreach (var checklist in checklists)
            {
                if (!string.IsNullOrEmpty(checklist.ScientificNameWithAuthor))
                {
                    try
                    {
                        var plantInfo = transformer.Transform(checklist);
                        plantInfos.Add(plantInfo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unable to process {checklist.Symbol} {checklist.CommonName} {checklist.ScientificNameWithAuthor} {ex.Message}", ex);
                    }
                }
            }

            var plantInfosResult = await processor.Process(plantInfos);
            foreach (var plantInfoResult in plantInfosResult)
            {
                _logger.LogInformation("CommonName" + ": " + plantInfoResult.CommonName + " ScientificName" + ": " + plantInfoResult.ScientificName +
                                       " PlantInfoId" + ": " + plantInfoResult.PlantInfoId);
            }
        }

        private async Task ProcessTaxonomicUnits(ITISPlantInfoTransformer transformer, PlantInfoProcessor processor, List<TaxonomicUnit> taxonomicUnits)
        {
            var plantInfos = new List<PlantInfo>();
            foreach (var taxonomicUnit in taxonomicUnits.GroupBy(t => t.Tsn))
            {
                var species = taxonomicUnit.First();
                if (species != null)
                {
                    try
                    {
                        var plantInfoResults = transformer.Transform(taxonomicUnit);
                        plantInfos.AddRange(plantInfoResults);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unable to process {taxonomicUnit.Key} {species} {ex.Message}", ex);
                    }
                }
            }

            var plantInfosResult = await processor.Process(plantInfos);
            foreach (var plantInfoResult in plantInfosResult)
            {
                _logger.LogInformation("CommonName" + ": " + plantInfoResult.CommonName + " ScientificName" + ": " + plantInfoResult.ScientificName +
                                       " PlantInfoId" + ": " + plantInfoResult.PlantInfoId);
            }
        }

        private async Task ProcessSynonyms(ITISSynonymTransformer transformer, SynonymProcessor processor, List<Vernacular> vernaculars)
        {
            if (vernaculars.Any())
            {
                var synonyms = new List<Synonym>();
                foreach (var vernacular in vernaculars)
                {
                    try
                    {
                        var synonym = transformer.Transform(vernacular);
                        synonyms.Add(synonym);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unable to process {vernacular.Taxon} {vernacular.Name} {vernacular.Rank} {ex.Message}", ex);
                    }
                }

                var synonymsResult = await processor.Process(synonyms);
                foreach (var synonymResult in synonymsResult)
                {
                    _logger.LogInformation("TaxonId" + ": " + synonymResult.Taxon.TaxonId + " Synonym" + ": " + synonymResult.Name +
                                           " Rank" + ": " + synonymResult.Rank);
                }
            }
        }

        private static IEnumerable<ImporterConfiguration> LoadImporters(IConfiguration configuration)
        {
            var importers = configuration.GetSection("importers").GetChildren();
            foreach (var importer in importers)
            {
                var config = new ImporterConfiguration
                {
                    Name = importer["name"],
                    Type = Enum.Parse<ImporterType>(importer["type"]),
                    IsActive = bool.Parse(importer["isActive"]),
                    ImportModel = importer["importModel"]
                };

                if (config.Type == ImporterType.TextImporter)
                {
                    config.Filename = importer["filename"];
                    config.HasHeaders = bool.Parse(importer["hasHeaders"]);
                }
                else if (config.Type == ImporterType.SqlImporter)
                {
                    config.ConnectionString = importer["connectionString"];
                    config.SqlQuery = importer["sqlQuery"];
                }
                else if (config.Type == ImporterType.JsonImporter)
                {
                    config.Filename = importer["filename"];
                }
                if (true)
                {
                    yield return config;
                }
            }
        }
    }
}
