using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.Modal;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class PlantInfoComponent : ComponentBase
    {
        [Inject]
        protected HttpClient Client { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public int Id { get; set; }
        public PlantInfo PlantInfo { get; set; }
        public IEnumerable<LightType> LightTypes => Enum.GetValues(typeof(LightType)).Cast<LightType>();
        public IEnumerable<WaterType> WaterTypes => Enum.GetValues(typeof(WaterType)).Cast<WaterType>();
        public IEnumerable<SoilType> SoilTypes => Enum.GetValues(typeof(SoilType)).Cast<SoilType>();
        public IEnumerable<ScarificationType> ScarificationTypes => Enum.GetValues(typeof(ScarificationType)).Cast<ScarificationType>();
        public IEnumerable<Month> Months => Enum.GetValues(typeof(Month)).Cast<Month>();
        public IEnumerable<DistanceUnit> DistanceUnits => Enum.GetValues(typeof(DistanceUnit)).Cast<DistanceUnit>();
        public IEnumerable<TemperatureUnit> TemperatureUnits => Enum.GetValues(typeof(TemperatureUnit)).Cast<TemperatureUnit>();
        public List<SoilType> ChosenSoilTypes;
        public List<ScarificationType> ChosenScarificationTypes = new List<ScarificationType>();
        public Dictionary<int, StratificationStage> ChosenStratificationStages = new Dictionary<int, StratificationStage>();
        public bool AnyStratificationStages() => PlantInfo.Requirements.StratificationRequirements.StratificationStages.Any() || ChosenStratificationStages.Any();

        protected override async Task OnInitializedAsync()
        {
            ChosenSoilTypes = new List<SoilType>();
            if (Id > 0)
            {
                PlantInfo = await Client.GetFromJsonAsync<PlantInfo>($"/api/plantinfo/{Id}");
            }
            else
            {
                PlantInfo = new PlantInfo
                {
                    Taxon = new Taxon(),
                    Origin = new Origin(),
                    Requirements = new Requirements
                    {
                        WaterRequirements = new WaterRequirements(),
                        LightRequirements = new LightRequirements(),
                        SoilRequirements = new List<SoilType>(),
                        ZoneRequirements = new ZoneRequirements { MinimumZone = new Zone(), MaximumZone = new Zone() },
                        ScarificationRequirements = new ScarificationRequirements(),
                        StratificationRequirements = new StratificationRequirements
                        {
                            StratificationStages = new Dictionary<int, StratificationStage>()
                        },
                        SeedStorageRequirements = new SeedStorageRequirements()
                    },
                    BloomTime = new BloomTime(),
                    Spread = new Spread(),
                    Height = new Height()
                };
            }
        }

        protected bool IsSoilTypeChosen(SoilType soilType) => ChosenSoilTypes.Any(s => s == soilType);

        protected void AddSoilType(SoilType soilType) => ChosenSoilTypes.Add(soilType);

        protected bool IsScarificationTypeChosen(ScarificationType scarificationType) => ChosenScarificationTypes.Any(s => s == scarificationType);

        protected void AddScarificationType(ScarificationType scarificationType) => ChosenScarificationTypes.Add(scarificationType);

        protected void AddStratificationStage(StratificationStage stratificationStage = null)
        {
            if (stratificationStage == null)
            {
                stratificationStage = new StratificationStage();
            }

            var position = ChosenStratificationStages.Count + 1;
            ChosenStratificationStages.Add(position, stratificationStage);
        }

        protected void RemoveStratificationStage(int key) => ChosenStratificationStages.Remove(key);
    }
}
