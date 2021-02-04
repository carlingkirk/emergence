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

        public ActivityService(IRepository<Activity> activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Data.Shared.Models.Activity> GetActivityAsync(int id, Data.Shared.Models.User user)
        {
            var activityQuery = _activityRepository.WhereWithIncludes(a => a.Id == id, false,
                                                                      a => a.Include(a => a.Specimen)
                                                                            .Include(a => a.Specimen.InventoryItem)
                                                                            .Include(a => a.Specimen.Lifeform)
                                                                            .Include(a => a.User)
                                                                            .Include(a => a.User.Photo));
            activityQuery = activityQuery.CanViewContent(user);

            var activity = await activityQuery.FirstOrDefaultAsync();

            return activity?.AsModel();
        }

        public async Task<Data.Shared.Models.Activity> AddOrUpdateActivityAsync(Data.Shared.Models.Activity activity)
        {
            var activityResult = await _activityRepository.AddOrUpdateAsync(a => a.Id == activity.ActivityId, activity.AsStore());
            return activityResult.AsModel();
        }

        public async Task<FindResult<Data.Shared.Models.Activity>> FindActivities(FindParams findParams, Data.Shared.Models.User user, int? specimenId = null)
        {
            var activityQuery = GetActivityQuery(findParams, user, specimenId);

            return await FindResult(activityQuery, findParams);
        }

        public async Task<FindResult<Data.Shared.Models.Activity>> FindScheduledActivities(FindParams findParams, Data.Shared.Models.User user, DateTime date)
        {
            var activityQuery = GetActivityQuery(findParams, user);

            activityQuery = activityQuery.Where(a => a.DateScheduled >= date);

            return await FindResult(activityQuery, findParams);
        }

        public async Task RemoveActivityAsync(Data.Shared.Models.Activity activity) => await _activityRepository.RemoveAsync(activity.AsStore());

        private IQueryable<Activity> GetActivityQuery(FindParams findParams, Data.Shared.Models.User user, int? specimenId = null)
        {
            var activityQuery = _activityRepository.WhereWithIncludes(a => (!specimenId.HasValue || a.SpecimenId == specimenId) &&
                                                       (!findParams.ContactsOnly || a.User.Contacts.Any(u => u.ContactUserId == user.Id)) &&
                                                       (findParams.SearchTextQuery == null ||
                                                        EF.Functions.Like(a.Name, findParams.SearchTextQuery) ||
                                                        EF.Functions.Like(a.Specimen.InventoryItem.Name, findParams.SearchTextQuery) ||
                                                        (a.Specimen.Lifeform != null && EF.Functions.Like(a.Specimen.Lifeform.CommonName, findParams.SearchTextQuery)) ||
                                                        (a.Specimen.Lifeform != null && EF.Functions.Like(a.Specimen.Lifeform.ScientificName, findParams.SearchTextQuery))),
                                                  false,
                                                  a => a.Include(a => a.Specimen)
                                                        .Include(a => a.Specimen.InventoryItem)
                                                        .Include(a => a.Specimen.Lifeform)
                                                        .Include(a => a.User));
            activityQuery = activityQuery.CanViewContent(user);

            if (!findParams.ContactsOnly && !string.IsNullOrEmpty(findParams.CreatedBy))
            {
                activityQuery = activityQuery.Where(a => a.CreatedBy == findParams.CreatedBy);
            }

            activityQuery = OrderBy(activityQuery, findParams.SortBy, findParams.SortDirection);

            return activityQuery;
        }

        private async Task<FindResult<Data.Shared.Models.Activity>> FindResult(IQueryable<Activity> activityQuery, FindParams findParams)
        {
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

        private IQueryable<Activity> OrderBy(IQueryable<Activity> activityQuery, string sortBy = "DateCreated", SortDirection sortDirection = SortDirection.Descending)
        {
            if (sortDirection == SortDirection.None)
            {
                return activityQuery;
            }

            var activitySorts = new Dictionary<string, Expression<Func<Activity, object>>>
            {
                { "Name", a => a.Name },
                { "ScientificName", a => a.Specimen.Lifeform != null ? a.Specimen.Lifeform.ScientificName : "" },
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
