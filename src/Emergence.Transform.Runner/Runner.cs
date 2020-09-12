using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.External.ITIS;
using Emergence.Data.External.USDA;
using Emergence.Data.Shared.Models;
using Microsoft.EntityFrameworkCore.Internal;
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
                    var processor = _importTransformOrchestrator.PlantInfoProcessor;
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

                                checklists.Clear();
                            }
                        }
                    }
                }
                else if (importer.Type == ImporterType.SqlImporter && importer.ImportModel == "TaxonomicUnit")
                {
                    var processor = _importTransformOrchestrator.PlantInfoProcessor;
                    var sqlImporter = new SqlImporter<TaxonomicUnit>(importer.ConnectionString, importer.SqlQuery);
                    var transformer = new ITISPlantInfoTransformer();
                    var startRow = 178498;
                    var batchSize = 500;
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

                                taxonomicUnits.Clear();
                            }
                        }
                    }
                }
                else if (importer.Type == ImporterType.SqlImporter && importer.ImportModel == "Vernacular")
                {
                    var processor = _importTransformOrchestrator.SynonymProcessor;
                    var sqlImporter = new SqlImporter<Vernacular>(importer.ConnectionString, importer.SqlQuery);
                    var transformer = new ITISSynonymTransformer();
                    var startRow = 1;
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
                            continue;
                        }
                        else if (row % batchSize != 0)
                        {
                            vernaculars.Add(result);
                        }
                        else
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

                                vernaculars.Clear();
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<ImporterConfiguration> LoadImporters(IConfiguration configuration)
        {
            var importers = configuration.GetSection("importers").GetChildren();
            foreach (var importer in importers)
            {
                var config = new ImporterConfiguration
                {
                    Name = importer["name"],
                    Type = Enum.Parse<ImporterType>(importer["type"]),
                    IsActive = bool.Parse(importer["isActive"])
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
                if (true)
                {
                    yield return config;
                }
            }
        }
    }
}
