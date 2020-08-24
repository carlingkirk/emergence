using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class PlantInfoComponent : ViewerComponent
    {
        [Parameter]
        public PlantInfo PlantInfo { get; set; }
        public Origin SelectedOrigin { get; set; }
        public Lifeform SelectedLifeform { get; set; }
        public string OriginSearch { get; set; }
        public IEnumerable<LightType> LightTypes => Enum.GetValues(typeof(LightType)).Cast<LightType>();
        public IEnumerable<WaterType> WaterTypes => Enum.GetValues(typeof(WaterType)).Cast<WaterType>();
        public IEnumerable<SoilType> SoilTypes => Enum.GetValues(typeof(SoilType)).Cast<SoilType>();
        public IEnumerable<Month> Months => Enum.GetValues(typeof(Month)).Cast<Month>();
        public IEnumerable<DistanceUnit> DistanceUnits => Enum.GetValues(typeof(DistanceUnit)).Cast<DistanceUnit>();
        public IEnumerable<StratificationType> StratificationTypes => Enum.GetValues(typeof(StratificationType)).Cast<StratificationType>();
        public List<SoilType> ChosenSoilTypes;
        public LinkedList<StratificationStage> ChosenStratificationStages = new LinkedList<StratificationStage>();
        public bool AnyStratificationStages() => PlantInfo.Requirements.StratificationStages != null && (PlantInfo.Requirements.StratificationStages.Any() || ChosenStratificationStages.Any());

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ChosenSoilTypes = new List<SoilType>();
            if (Id > 0 || PlantInfo != null)
            {
                PlantInfo ??= await ApiClient.GetPlantInfoAsync(Id);
                SelectedLifeform = PlantInfo.Lifeform;
                SelectedOrigin = PlantInfo.Origin;

                PlantInfo.Requirements.ZoneRequirements.MinimumZone ??= new Zone();
                PlantInfo.Requirements.ZoneRequirements.MaximumZone ??= new Zone();
                if (PlantInfo.Requirements.StratificationStages != null)
                {
                    PlantInfo.Requirements.StratificationStages.OrderBy(s => s.Step).ToList().ForEach(s =>
                    {
                        ChosenStratificationStages.AddLast(s);
                    });
                }
            }
            else
            {
                IsEditing = true;
                PlantInfo = new PlantInfo
                {
                    Origin = new Origin(),
                    Requirements = new Requirements
                    {
                        WaterRequirements = new WaterRequirements(),
                        LightRequirements = new LightRequirements(),
                        SoilRequirements = new List<SoilType>(),
                        ZoneRequirements = new ZoneRequirements { MinimumZone = new Zone(), MaximumZone = new Zone() },
                        StratificationStages = new List<StratificationStage>
                        {

                        }
                    },
                    BloomTime = new BloomTime(),
                    Spread = new Spread(),
                    Height = new Height()
                };
            }

            if (!string.IsNullOrEmpty(UserId) && PlantInfo.CreatedBy == UserId)
            {
                IsEditable = true;
            }
        }

        protected bool IsSoilTypeChosen(SoilType soilType) => ChosenSoilTypes.Any(s => s == soilType);

        protected async Task<IEnumerable<Lifeform>> FindLifeformsAsync(string searchText) => await ApiClient.FindLifeformsAsync(new FindParams
        {
            SearchText = searchText,
            Skip = 0,
            Take = 10,
            SortBy = "ScientificName",
            SortDirection = SortDirection.Ascending
        });

        protected string GetElementId(string element, string id) => element + "-" + id;
    }
}
