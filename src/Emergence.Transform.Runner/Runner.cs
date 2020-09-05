using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.External.ITIS;
using Emergence.Data.External.USDA;
using Emergence.Data.Shared.Models;
using Emergence.Transform.USDA;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Emergence.Transform.Runner
{
    public class Runner
    {
        private readonly ILogger<Runner> _logger;
        private readonly IUSDAProcessor _USDAProcessor;
        private readonly IConfiguration Configuration;

        public Runner(ILogger<Runner> logger, IConfiguration configuration, IUSDAProcessor uSDAProcessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _USDAProcessor = uSDAProcessor;
            Configuration = configuration;
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
                if (importer.Type == ImporterType.TextImporter)
                {
                    var transformer = new USDATransformer();
                    var startRow = 1;
                    var batchSize = 100;

                    await _USDAProcessor.InitializeOrigin(transformer.Origin);
                    await _USDAProcessor.InitializeLifeforms();
                    await _USDAProcessor.InitializeTaxons();

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

                                var plantInfosResult = await _USDAProcessor.Process(plantInfos);
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
                else if (importer.Type == ImporterType.SqlImporter)
                {
                    var sqlImporter = new SqlImporter<TaxonomicUnit>(importer.ConnectionString, importer.SqlQuery);
                    var transformer = new ITISTransformer();
                    var startRow = 1;
                    var batchSize = 100;

                    var row = 1;
                    var taxonomicUnits = new List<TaxonomicUnit>();
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
