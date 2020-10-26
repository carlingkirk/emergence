using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Data.Shared.Models.Activity> GetActivityAsync(int id, Data.Shared.Models.User user)
        {
            var result = await _activityRepository.GetAsync(a => a.Id == id && a.CanViewContent(user.AsStore()));
            var activity = result?.AsModel();

            if (result.SpecimenId.HasValue)
            {
                var specimen = await _specimenService.GetSpecimenAsync(result.SpecimenId.Value, user);
                activity.Specimen = specimen;
            }

            return activity;
        }

        public async Task<Data.Shared.Models.Activity> AddOrUpdateActivityAsync(Data.Shared.Models.Activity activity, string userId)
        {
            activity.CreatedBy = userId;
            var activityResult = await _activityRepository.AddOrUpdateAsync(a => a.Id == activity.ActivityId, activity.AsStore());
            return activityResult.AsModel();
        }

        public async Task<FindResult<Data.Shared.Models.Activity>> FindActivities(FindParams findParams, Data.Shared.Models.User user, int? specimenId = 0)
        {
            var activityQuery = _activityRepository.WhereWithIncludes(a => (!specimenId.HasValue ||
                                                                            a.SpecimenId == specimenId) &&
                                                                           (findParams.SearchTextQuery == null ||
                                                                            EF.Functions.Like(a.Name, findParams.SearchTextQuery) ||
                                                                            EF.Functions.Like(a.Specimen.InventoryItem.Name, findParams.SearchTextQuery) ||
                                                                            EF.Functions.Like(a.Specimen.Lifeform.CommonName, findParams.SearchTextQuery) ||
                                                                            EF.Functions.Like(a.Specimen.Lifeform.ScientificName, findParams.SearchTextQuery)),
                                                                      a => a.Include(a => a.Specimen)
                                                                            .Include(s => s.Specimen.InventoryItem)
                                                                            .Include(s => s.Specimen.Lifeform));

            activityQuery = activityQuery.Where(a => a.CanViewContent(user.AsStore()));

            if (!string.IsNullOrEmpty(findParams.CreatedBy))
            {
                activityQuery = activityQuery.Where(a => a.CreatedBy == findParams.CreatedBy);
            }

            activityQuery = OrderBy(activityQuery, findParams.SortBy, findParams.SortDirection);

            var count = activityQuery.Count();
            var activityResult = activityQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var activities = new List<Data.Shared.Models.Activity>();
            await foreach (var activity in activityResult)
            {
                activities.Add(activity.AsModel());
            }
            return new FindResult<Data.Shared.Models.Activity>
            {
                Count = count,
                Results = activities
            };
        }

        public async Task RemoveActivityAsync(Data.Shared.Models.Activity activity) => await _activityRepository.RemoveAsync(activity.AsStore());

        private IQueryable<Activity> OrderBy(IQueryable<Activity> activityQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return activityQuery;
            }

            if (sortBy == null)
            {
                sortBy = "DateCreated";
            }

            var activitySorts = new Dictionary<string, Expression<Func<Activity, object>>>
            {
                { "Name", a => a.Name },
                { "ScientificName", a => a.Specimen.Lifeform.ScientificName },
                { "ActivityType", a => a.ActivityType },
                { "DateOccured", a => a.DateOccurred },
                { "DateScheduled", a => a.DateScheduled },
                { "DateCreated", a => a.DateCreated }
            };

            if (sortDirection == SortDirection.Descending)
            {
                activityQuery = activityQuery.WithOrder(a => a.OrderByDescending(activitySorts[sortBy]));
            }
            else
            {
                activityQuery = activityQuery.WithOrder(a => a.OrderBy(activitySorts[sortBy]));
            }

            return activityQuery;
        }
    }
}
