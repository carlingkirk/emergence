using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IActivityService
    {
        Task<Activity> GetActivityAsync(int id, User user);
        Task<Activity> AddOrUpdateActivityAsync(Activity activity, string userId);
        Task<FindResult<Activity>> FindActivities(FindParams findParams, User user, int? specimenId = 0);
        Task RemoveActivityAsync(Activity activity);
    }
}
