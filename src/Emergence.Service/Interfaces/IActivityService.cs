using System;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface IActivityService
    {
        Task<Activity> GetActivityAsync(int id, User user);
        Task<Activity> AddOrUpdateActivityAsync(Activity activity);
        Task<FindResult<Activity>> FindActivities(FindParams findParams, User user, int? specimenId = null);
        Task<FindResult<Activity>> FindScheduledActivities(FindParams findParams, User user, DateTime date);
        Task RemoveActivityAsync(Activity activity);
    }
}
