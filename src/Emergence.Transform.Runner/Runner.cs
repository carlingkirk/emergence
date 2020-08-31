using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.External.USDA;
using Emergence.Transform.USDA;
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

            var transformer = new USDATransformer();
            var startRow = 54000;
            var batchSize = 100;

            await _USDAProcessor.InitializeOrigin(transformer.Origin);
            await _USDAProcessor.InitializeLifeforms();
            await _USDAProcessor.InitializeTaxons();

            foreach (var importer in importers)
            {
                if (importer.Type == ImporterType.TextImporter)
                {
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
                            foreach (var checklist in checklists)
                            {
                                if (!string.IsNullOrEmpty(checklist.ScientificNameWithAuthor))
                                {
                                    try
                                    {
                                        var plantInfo = transformer.Transform(checklist);
                                        var plantInfoResult = await _USDAProcessor.Process(plantInfo);

                                        _logger.LogInformation("CommonName" + ": " + plantInfoResult.CommonName + " ScientificName" + ": " + plantInfoResult.ScientificName +
                                                           " PlantInfoId" + ": " + plantInfoResult.PlantInfoId);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError($"Unable to process {checklist.Symbol} {checklist.CommonName} {checklist.ScientificNameWithAuthor} {ex.Message}", ex);
                                    }
                                }
                            }

                            checklists.Clear();
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
                yield return new ImporterConfiguration
                {
                    Name = importer["name"],
                    Type = Enum.Parse<ImporterType>(importer["type"]),
                    Filename = importer["filename"],
                    HasHeaders = bool.Parse(importer["hasHeaders"])
                };
            }
        }
    }
}
