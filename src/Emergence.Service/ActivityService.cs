using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;

namespace Emergence.Service
{
    public class ActivityService : IActivityService
    {
        private readonly IRepository<Activity> _activityRepository;
        private readonly ISpecimenService _specimenService;
        private readonly IInventoryService _inventoryService;

        public ActivityService(IRepository<Activity> activityRepository, ISpecimenService specimenService, IInventoryService inventoryService)
        {
            _activityRepository = activityRepository;
            _specimenService = specimenService;
            _inventoryService = inventoryService;
        }

        public async Task<IEnumerable<Data.Shared.Models.Activity>> GetActivitiesAsync()
        {
            var activitiesResult = _activityRepository.GetSomeAsync(a => a.Id > 0);

            var activities = new List<Data.Shared.Models.Activity>();
            await foreach (var activity in activitiesResult)
            {
                activities.Add(activity.AsModel());
            }

            var specimens = await _specimenService.GetSpecimensByIdsAsync(activities.Select(a => a.Specimen.SpecimenId));
            foreach (var activity in activities)
            {
                var specimen = specimens.Where(s => s.SpecimenId == activity.Specimen.SpecimenId).First();
                activity.Specimen = specimen;
            }

            var inventoryItems = await _inventoryService.GetInventoryItemsByIdsAsync(specimens.Select(s => s.InventoryItem.InventoryItemId));
            foreach (var activity in activities)
            {
                var inventoryItem = inventoryItems.Where(i => i.InventoryItemId == activity.Specimen.InventoryItem.InventoryItemId).First();
                activity.Specimen.InventoryItem = inventoryItem;
            }

            return activities;
        }

        public async Task<Data.Shared.Models.Activity> GetActivityAsync(int id)
        {
            var result = await _activityRepository.GetAsync(a => a.Id == id);
            var activity = result?.AsModel();
            return activity;
        }

        public async Task<Data.Shared.Models.Activity> AddOrUpdateActivityAsync(Data.Shared.Models.Activity activity)
        {
            var activityResult = await _activityRepository.AddOrUpdateAsync(a => a.Id == activity.ActivityId, activity.AsStore());
            return activityResult.AsModel();
        }
    }
}
