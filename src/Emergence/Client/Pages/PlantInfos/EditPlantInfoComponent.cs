using System;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class EditPlantInfoComponent : PlantInfoComponent
    {
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public Func<Task> Cancel { get; set; }

        protected async Task SavePlantInfoAsync()
        {
            var isNewPlantInfo = PlantInfo.PlantInfoId == 0;
            if (isNewPlantInfo)
            {
                PlantInfo.DateCreated = DateTime.UtcNow;
            }
            else
            {
                PlantInfo.DateModified = DateTime.UtcNow;
            }

            PlantInfo.Origin = SelectedOrigin;
            PlantInfo.Photos = UploadedPhotos.Any() ? UploadedPhotos : null;
            PlantInfo.Lifeform = PlantInfo.SelectedLifeform;
            PlantInfo.CreatedBy = UserId;
            PlantInfo.Requirements.StratificationStages = ChosenStratificationStages != null && ChosenStratificationStages.Any() ? ChosenStratificationStages.ToList() : null;
            PlantInfo.WildlifeEffects = ChosenWildlifeEffects != null && ChosenWildlifeEffects.Any() ? ChosenWildlifeEffects.ToList() : null;
            PlantInfo.SoilTypes = ChosenSoilTypes != null && ChosenSoilTypes.Any() ? ChosenSoilTypes.ToList() : null;

            if (PlantInfo.Height.MinimumHeight.HasValue || PlantInfo.Height.MaximumHeight.HasValue)
            {
                PlantInfo.Height.Unit = DistanceUnit.Feet;
            }
            else
            {
                PlantInfo.Height.Unit = DistanceUnit.Unknown;
            }

            if (PlantInfo.Spread.MinimumSpread.HasValue || PlantInfo.Spread.MaximumSpread.HasValue)
            {
                PlantInfo.Spread.Unit = DistanceUnit.Feet;
            }
            else
            {
                PlantInfo.Spread.Unit = DistanceUnit.Unknown;
            }

            if (MinimumZoneId != PlantInfo.Requirements.ZoneRequirements?.MinimumZone?.Id)
            {
                PlantInfo.Requirements.ZoneRequirements = new ZoneRequirements
                {
                    MinimumZone = Zones.First(z => z.Id == MinimumZoneId)
                };
            }

            if (MaximumZoneId != PlantInfo.Requirements.ZoneRequirements?.MaximumZone?.Id)
            {
                if (PlantInfo.Requirements.ZoneRequirements == null)
                {
                    PlantInfo.Requirements.ZoneRequirements = new ZoneRequirements
                    {
                        MaximumZone = Zones.First(z => z.Id == MaximumZoneId)
                    };
                }
                else
                {
                    PlantInfo.Requirements.ZoneRequirements.MaximumZone = Zones.First(z => z.Id == MaximumZoneId);
                }
            }

            PlantInfo = await ApiClient.PutPlantInfoAsync(PlantInfo);

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(PlantInfo));
            }
            else
            {
                await CancelAsync(isNewPlantInfo);
            }
        }

        protected async Task CancelAsync(bool isNewPlantInfo = false)
        {
            if (PlantInfo.PlantInfoId == 0 || isNewPlantInfo)
            {
                if (isNewPlantInfo)
                {
                    await RefreshListAsync();
                }

                await Cancel.Invoke();
            }
            else
            {
                await IsEditingChanged.InvokeAsync(false);
                await RefreshListAsync();
            }
        }

        protected void AddRemoveSoilType(SoilType soilType)
        {
            if (ChosenSoilTypes.Contains(soilType))
            {
                ChosenSoilTypes.Remove(soilType);
            }
            else
            {
                ChosenSoilTypes.Add(soilType);
            }
        }

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

        protected void AddWildlifeEffect(WildlifeEffect wildlifeEffect = null)
        {
            if (wildlifeEffect == null)
            {
                wildlifeEffect = new WildlifeEffect();
            }

            ChosenWildlifeEffects.Add(wildlifeEffect);
        }

        protected void RemoveWildlifeEffect(WildlifeEffect wildlifeEffect) => _ = ChosenWildlifeEffects.Remove(wildlifeEffect);

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
