using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class PlantInfoComponent : ViewerComponent<PlantInfo>
    {
        [Parameter]
        public PlantInfo PlantInfo { get; set; }
        public Origin SelectedOrigin { get; set; }
        public string MinZone { get; set; }
        public string MaxZone { get; set; }
        public List<Photo> UploadedPhotos { get; set; }
        public IEnumerable<LightType> LightTypes => Enum.GetValues(typeof(LightType)).Cast<LightType>();
        public IEnumerable<WaterType> WaterTypes => Enum.GetValues(typeof(WaterType)).Cast<WaterType>();
        public IEnumerable<SoilType> SoilTypes => Enum.GetValues(typeof(SoilType)).Cast<SoilType>();
        public IEnumerable<Month> Months => Enum.GetValues(typeof(Month)).Cast<Month>();
        public IEnumerable<DistanceUnit> DistanceUnits => Enum.GetValues(typeof(DistanceUnit)).Cast<DistanceUnit>();
        public IEnumerable<StratificationType> StratificationTypes => Enum.GetValues(typeof(StratificationType)).Cast<StratificationType>();
        public List<SoilType> ChosenSoilTypes;
        public LinkedList<StratificationStage> ChosenStratificationStages = new LinkedList<StratificationStage>();
        public bool AnyStratificationStages() => PlantInfo.Requirements.StratificationStages != null && (PlantInfo.Requirements.StratificationStages.Any() || ChosenStratificationStages.Any());
        public string CommonName => PlantInfo?.CommonName ?? PlantInfo.Lifeform?.CommonName ?? "";
        public string ScientificName => PlantInfo?.ScientificName ?? PlantInfo.Lifeform?.ScientificName ?? "";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ChosenSoilTypes = new List<SoilType>();
            if (Id > 0 || PlantInfo != null)
            {
                PlantInfo ??= await ApiClient.GetPlantInfoAsync(Id);

                if (PlantInfo.Photos != null && PlantInfo.Photos.Any())
                {
                    UploadedPhotos = PlantInfo.Photos.ToList();
                }
                else
                {
                    UploadedPhotos = new List<Photo>();
                }

                PlantInfo.SelectedLifeform = PlantInfo.Lifeform;
                SelectedOrigin = PlantInfo.Origin;

                MinZone = PlantInfo.Requirements.ZoneRequirements.MinimumZone?.ToFriendlyString();
                MaxZone = PlantInfo.Requirements.ZoneRequirements.MaximumZone?.ToFriendlyString();

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
                    Origin = null,
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
                    Height = new Height(),
                    Photos = new List<Photo>()
                };
            }
        }

        protected bool IsSoilTypeChosen(SoilType soilType) => ChosenSoilTypes.Any(s => s == soilType);

        protected async Task<IEnumerable<Lifeform>> FindLifeformsAsync(string searchText)
        {
            var result = await ApiClient.FindLifeformsAsync(new FindParams
            {
                SearchText = searchText,
                Skip = 0,
                Take = 10,
                SortBy = "ScientificName",
                SortDirection = SortDirection.Ascending
            });

            return result.Results;
        }

        protected string GetElementId(string element, string id) => element + "-" + id;

        protected async Task RemovePlantInfo()
        {
            var result = await ApiClient.RemovePlantInfoAsync(PlantInfo);
            if (result)
            {
                PlantInfo = null;

                await RefreshListAsync();
                await UnloadItem();
            }
        }
    }
}
