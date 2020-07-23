using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
{
    public class ActivityService : IActivityService
    {
        private readonly IRepository<Data.Shared.Stores.Activity> _activityRepository;
        private readonly ISpecimenService _specimenService;
        private readonly IInventoryService _inventoryService;

        public ActivityService(IRepository<Data.Shared.Stores.Activity> activityRepository, ISpecimenService specimenService, IInventoryService inventoryService)
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

            if (result.SpecimenId.HasValue)
            {
                var specimen = await _specimenService.GetSpecimenAsync(result.SpecimenId.Value);
                activity.Specimen = specimen;
            }

            return activity;
        }

        public async Task<Data.Shared.Models.Activity> AddOrUpdateActivityAsync(Data.Shared.Models.Activity activity, string userId)
        {
            activity.UserId = userId;
            var activityResult = await _activityRepository.AddOrUpdateAsync(a => a.Id == activity.ActivityId, activity.AsStore());
            return activityResult.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Activity>> FindActivities(string search, string userId, int skip, int take)
        {
            if (search != null)
            {
                search = "%" + search + "%";
            }

            var activityResult = _activityRepository.WhereWithIncludesAsync(a => (a.Specimen.InventoryItem.Inventory.UserId == userId) &&
                                                                       (search == null ||
                                                                       EF.Functions.Like(a.Name, search) ||
                                                                        EF.Functions.Like(a.Specimen.InventoryItem.Name, search)),
                                                                        a => a.Include(a => a.Specimen)
                                                                              .Include(s => s.Specimen.InventoryItem)
                                                                              .Include(s => s.Specimen.Lifeform))
                                                    .WithOrder(s => s.OrderByDescending(s => s.DateCreated))
                                                    .GetSomeAsync(skip: skip, take: take, track: false);

            var activities = new List<Data.Shared.Models.Activity>();
            await foreach (var activity in activityResult)
            {
                activities.Add(activity.AsModel());
            }
            return activities;
        }
    }
}
