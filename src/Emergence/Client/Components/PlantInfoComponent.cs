using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
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
        public IEnumerable<Month> Months => Enum.GetValues(typeof(Month)).Cast<Month>();
        public IEnumerable<DistanceUnit> DistanceUnits => Enum.GetValues(typeof(DistanceUnit)).Cast<DistanceUnit>();
        public IEnumerable<StratificationType> StratificationTypes => Enum.GetValues(typeof(StratificationType)).Cast<StratificationType>();
        public List<SoilType> ChosenSoilTypes;
        public LinkedList<StratificationStage> ChosenStratificationStages = new LinkedList<StratificationStage>();
        public bool AnyStratificationStages() => PlantInfo.Requirements.StratificationStages.Any() || ChosenStratificationStages.Any();

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
                        StratificationStages = new List<StratificationStage>
                        {

                        }
                    },
                    BloomTime = new BloomTime(),
                    Spread = new Spread(),
                    Height = new Height()
                };
            }
        }

        protected async Task SavePlantInfo()
        {
            if (PlantInfo.PlantInfoId == 0)
            {
                PlantInfo.DateCreated = DateTime.UtcNow;
            }
            PlantInfo.DateModified = DateTime.UtcNow;

            var result = await Client.PutAsJsonAsync("/api/plantinfo", PlantInfo);
            if (result.IsSuccessStatusCode)
            {
                PlantInfo = await result.Content.ReadFromJsonAsync<PlantInfo>();
            }
            BlazoredModal.Close(ModalResult.Ok(PlantInfo));
        }

        protected bool IsSoilTypeChosen(SoilType soilType) => ChosenSoilTypes.Any(s => s == soilType);

        protected void AddSoilType(SoilType soilType) => ChosenSoilTypes.Add(soilType);

        protected void AddStratificationStage(StratificationStage stratificationStage = null)
        {
            if (stratificationStage == null)
            {
                stratificationStage = new StratificationStage
                {
                    Step = ChosenStratificationStages.Count + 1
                };
            }

            ChosenStratificationStages.AddLast(stratificationStage);
        }

        protected void RemoveStratificationStage(int step)
        {
            // if I remove step 2, I need step 3 to become step 2
            var removeStage = ChosenStratificationStages.First(s => s.Step == step);
            var afterStages = ChosenStratificationStages.OrderBy(s => s.Step).Skip(step).ToList();

            ChosenStratificationStages.Remove(removeStage);

            foreach (var afterStage in afterStages)
            {
                var stage = ChosenStratificationStages.First(s => s.Step == afterStage.Step);
                Console.WriteLine(stage.Step);
                stage.Step--;
                Console.WriteLine(stage.Step);
            }
        }

        protected void MoveStratificationStage(int oldStep, int newStep)
        {
            // if I move step 2 to step 3
            var oldStage = ChosenStratificationStages.First(s => s.Step == oldStep);
            var newStage = ChosenStratificationStages.First(s => s.Step == newStep);

            var oldNode = ChosenStratificationStages.Find(oldStage);
            var newNode = ChosenStratificationStages.Find(oldStage);

            if (oldStep < newStep)
            {
                oldStage.Step++;
                newStage.Step--;
                ChosenStratificationStages.AddAfter(oldNode, newNode);
            }
            else if (oldStep > newStep)
            {
                oldStage.Step--;
                newStage.Step++;
                ChosenStratificationStages.AddBefore(oldNode, newNode);
            }
        }
    }
}
