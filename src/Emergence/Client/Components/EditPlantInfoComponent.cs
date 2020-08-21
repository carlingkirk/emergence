using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class EditPlantInfoComponent : PlantInfoComponent
    {
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        public string MinZone { get; set; }
        public string MaxZone { get; set; }

        protected async Task SavePlantInfoAsync()
        {
            if (PlantInfo.PlantInfoId == 0)
            {
                PlantInfo.DateCreated = DateTime.UtcNow;
            }
            else
            {
                PlantInfo.DateModified = DateTime.UtcNow;
            }

            if (SelectedOrigin != null)
            {
                PlantInfo.Origin = SelectedOrigin;
            }
            PlantInfo.Lifeform = SelectedLifeform;

            if (!string.IsNullOrEmpty(MinZone))
            {
                var minZoneLetter = MinZone.Substring(0, 1);
                var minZoneNumber = MinZone.Length > 1 ? MinZone.Substring(1, 1) : null;
                int.TryParse(minZoneNumber, out var minZoneInt);

                PlantInfo.Requirements.ZoneRequirements.MinimumZone.Letter = minZoneLetter;
                PlantInfo.Requirements.ZoneRequirements.MinimumZone.Number = minZoneInt;
            }

            if (!string.IsNullOrEmpty(MaxZone))
            {
                var maxZoneLetter = MaxZone.Substring(0, 1);
                var maxZoneNumber = MaxZone.Length > 1 ? MinZone.Substring(1, 1) : null;
                int.TryParse(maxZoneNumber, out var maxZoneInt);
                PlantInfo.Requirements.ZoneRequirements.MinimumZone.Letter = maxZoneLetter;
                PlantInfo.Requirements.ZoneRequirements.MinimumZone.Number = maxZoneInt;
            }

            PlantInfo.Requirements.StratificationStages = ChosenStratificationStages.ToList();

            PlantInfo = await ApiClient.PutPlantInfoAsync(PlantInfo);

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(PlantInfo));
            }
            else
            {
                await IsEditingChanged.InvokeAsync(false);
                await IsItemLoadedChanged.InvokeAsync(false);
            }
        }

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
