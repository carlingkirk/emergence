using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IActivityService
    {
        Task<Activity> GetActivityAsync(int id);
        Task<IEnumerable<Activity>> GetActivitiesAsync();
        Task<Activity> AddOrUpdateActivityAsync(Activity activity, string userId);
        Task<FindResult<Activity>> FindActivities(string search, int? specimenId, string userId, int skip, int take, string sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending);
    }
}
