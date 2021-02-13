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
        public List<Photo> UploadedPhotos { get; set; }
        public static IEnumerable<LightType> LightTypes => Enum.GetValues(typeof(LightType)).Cast<LightType>();
        public static IEnumerable<WaterType> WaterTypes => Enum.GetValues(typeof(WaterType)).Cast<WaterType>();
        public static IEnumerable<SoilType> SoilTypes => Enum.GetValues(typeof(SoilType)).Cast<SoilType>();
        public static IEnumerable<Month> Months => Enum.GetValues(typeof(Month)).Cast<Month>();
        public static IEnumerable<DistanceUnit> DistanceUnits => Enum.GetValues(typeof(DistanceUnit)).Cast<DistanceUnit>();
        public static IEnumerable<StratificationType> StratificationTypes => Enum.GetValues(typeof(StratificationType)).Cast<StratificationType>();
        public static IEnumerable<Wildlife> WildlifeTypes => Enum.GetValues(typeof(Wildlife)).Cast<Wildlife>();
        public static IEnumerable<Effect> Effects => Enum.GetValues(typeof(Effect)).Cast<Effect>();
        public List<SoilType> ChosenSoilTypes { get; set; }
        public LinkedList<StratificationStage> ChosenStratificationStages { get; set; }
        public List<WildlifeEffect> ChosenWildlifeEffects { get; set; }
        public IEnumerable<Zone> Zones { get; set; }
        public int? MinimumZoneId { get; set; }
        public int? MaximumZoneId { get; set; }
        public string CommonName => PlantInfo?.CommonName ?? PlantInfo.Lifeform?.CommonName ?? "";
        public string ScientificName => PlantInfo?.ScientificName ?? PlantInfo.Lifeform?.ScientificName ?? "";
        protected bool IsOwner => !string.IsNullOrEmpty(UserId) && (PlantInfo?.CreatedBy == UserId);

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Zones = new List<Zone> { new Zone { Id = null, Name = "" } }.Concat(ZoneHelper.GetZones());
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
                MinimumZoneId = PlantInfo.Requirements.ZoneRequirements?.MinimumZone?.Id;
                MaximumZoneId = PlantInfo.Requirements.ZoneRequirements?.MaximumZone?.Id;

                if (PlantInfo.Requirements.StratificationStages != null)
                {
                    ChosenStratificationStages = new LinkedList<StratificationStage>();
                    PlantInfo.Requirements.StratificationStages.OrderBy(s => s.Step).ToList().ForEach(s =>
                    {
                        ChosenStratificationStages.AddLast(s);
                    });
                }
            }
            else
            {
                IsEditing = true;
                ChosenStratificationStages = new LinkedList<StratificationStage>();
                PlantInfo = new PlantInfo
                {
                    Origin = null,
                    Requirements = new Requirements
                    {
                        WaterRequirements = new WaterRequirements(),
                        LightRequirements = new LightRequirements(),
                        SoilRequirements = new List<SoilType>(),
                        ZoneRequirements = new ZoneRequirements { MinimumZone = new Zone(), MaximumZone = new Zone() },
                        StratificationStages = new List<StratificationStage>()
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
            var result = await ApiClient.FindLifeformsAsync(new FindParams<Lifeform>
            {
                SearchText = searchText,
                UseNGrams = true,
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

        protected bool AnyStratificationStages() =>
            (PlantInfo.Requirements?.StratificationStages != null && PlantInfo.Requirements.StratificationStages.Any()) ||
            (ChosenStratificationStages != null && ChosenStratificationStages.Any());

        protected bool AnyWildlifeEffects() =>
            (PlantInfo.WildlifeEffects != null && PlantInfo.WildlifeEffects.Any()) ||
            (ChosenWildlifeEffects != null && ChosenWildlifeEffects.Any());
    }
}
