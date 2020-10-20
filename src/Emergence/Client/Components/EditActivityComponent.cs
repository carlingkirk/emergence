using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Client.Common;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Emergence.Client.Components
{
    public class EditActivityComponent : ActivityComponent
    {
        [Inject]
        protected IModalServiceClient ModalServiceClient { get; set; }
        [CascadingParameter]
        protected BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter]
        public Func<Task> Cancel { get; set; }
        public bool UpdateSpecimen { get; set; }
        public bool IsNewSpecimen { get; set; }
        protected bool ShowAutoSpecimen => Activity.ActivityId == 0 && !IsNewSpecimen &&
            (Activity.ActivityType == ActivityType.PlantInGround ||
             Activity.ActivityType == ActivityType.Germination ||
             Activity.ActivityType == ActivityType.Stratification);

        public EditActivityComponent()
        {
            if (Id == 0)
            {
                UpdateSpecimen = true;
            }
        }

        protected override async Task OnInitializedAsync() => await base.OnInitializedAsync();

        protected async Task SaveActivityAsync()
        {
            var isNewActivity = Activity.ActivityId == 0;
            if (isNewActivity)
            {
                Activity.DateCreated = DateTime.UtcNow;
            }
            else
            {
                Activity.DateModified = DateTime.UtcNow;
            }

            if (string.IsNullOrEmpty(Activity.Name))
            {
                PopulateActivityName();
            }

            Activity.Photos = UploadedPhotos.Any() ? UploadedPhotos : null;

            if (SelectedSpecimen != null)
            {
                Activity.Specimen = SelectedSpecimen;
            }

            if (UpdateSpecimen && Activity.Quantity.HasValue)
            {
                var stage = GetSpecimenStage(Activity.ActivityType);

                if (Activity.Quantity == SelectedSpecimen.InventoryItem.Quantity)
                {
                    SelectedSpecimen.SpecimenStage = stage;
                }
                else
                {
                    var newSpecimen = new Specimen
                    {
                        SpecimenStage = stage,
                        Lifeform = SelectedSpecimen.Lifeform,
                        InventoryItem = new InventoryItem
                        {
                            Name = SelectedSpecimen.InventoryItem.Name,
                            ItemType = ItemType.Specimen,
                            Quantity = Activity.Quantity.Value,
                            Status = SelectedSpecimen.InventoryItem.Status,
                            Inventory = SelectedSpecimen.InventoryItem.Inventory,
                            Origin = SelectedSpecimen.InventoryItem.Origin,
                            DateAcquired = Activity.DateOccured,
                            DateCreated = DateTime.UtcNow,
                            CreatedBy = UserId
                        },
                        CreatedBy = UserId,
                        DateCreated = DateTime.UtcNow
                    };

                    newSpecimen = await ApiClient.PutSpecimenAsync(newSpecimen);

                    Activity.Specimen = newSpecimen;
                    SelectedSpecimen.InventoryItem.Quantity -= Activity.Quantity.Value;
                }

                await ApiClient.PutSpecimenAsync(SelectedSpecimen);
            }

            Activity = await ApiClient.PutActivityAsync(Activity);
            Id = Activity.ActivityId;

            if (BlazoredModal != null)
            {
                await BlazoredModal.Close(ModalResult.Ok(Activity));
            }
            else
            {
                await CancelAsync(isNewActivity);
            }
        }

        protected async Task CancelAsync(bool isNewActivity = false)
        {
            if (Activity.ActivityId == 0 || isNewActivity)
            {
                await Cancel.Invoke();

                if (isNewActivity)
                {
                    await RefreshListAsync();
                }
            }
            else
            {
                await IsEditingChanged.InvokeAsync(false);

                await RefreshListAsync();
            }
        }

        protected async Task<IEnumerable<Specimen>> FindSpecimensAsync(string searchText)
        {
            var specimenResult = await ApiClient.FindSpecimensAsync(new FindParams
            {
                SearchText = searchText,
                Skip = 0,
                Take = 10,
                SortBy = "ScientificName",
                SortDirection = SortDirection.Ascending
            });

            var specimens = specimenResult.Results.ToList();

            var result = await ApiClient.FindLifeformsAsync(new FindParams
            {
                SearchText = searchText,
                Skip = 0,
                Take = 3 + (10 - specimens.Count),
                SortBy = "ScientificName",
                SortDirection = SortDirection.Ascending
            });

            foreach (var lifeform in result.Results)
            {
                specimens.Add(new Specimen
                {
                    Lifeform = lifeform,
                    InventoryItem = new InventoryItem
                    {
                        Inventory = new Inventory
                        {
                            CreatedBy = UserId
                        }
                    }
                });
            }

            return specimens;
        }

        protected void PopulateActivityName()
        {
            if (string.IsNullOrEmpty(Activity.Name))
            {
                if (Activity.ActivityType != ActivityType.Custom)
                {
                    Activity.Name = Activity.ActivityType.ToFriendlyName() + ": " + SelectedSpecimen?.Lifeform?.ScientificName ?? "";
                }
                else
                {
                    Activity.Name = Activity.CustomActivityType + ": " + SelectedSpecimen?.Lifeform?.ScientificName ?? "";
                }
            }
        }

        protected async Task AddSpecimenAsync(Specimen specimen)
        {
            var result = await ModalServiceClient.ShowSpecimenModal(specimen, true);

            if (!result.Cancelled)
            {
                SelectedSpecimen = specimen;
                SelectedSpecimen.SpecimenId = ((Specimen)result.Data).SpecimenId;
                IsNewSpecimen = true;
            }
        }

        protected SpecimenStage GetSpecimenStage(ActivityType activityType)
        {
            switch (activityType)
            {
                case ActivityType.Germination:
                    return SpecimenStage.Germination;
                case ActivityType.Stratification:
                    return SpecimenStage.Stratification;
                case ActivityType.PlantInGround:
                    return SpecimenStage.InGround;
                default:
                    return SpecimenStage.Unknown;
            }
        }
    }
}
