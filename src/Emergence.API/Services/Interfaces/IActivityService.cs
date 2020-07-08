using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.API.Services.Interfaces
{
    public interface IActivityService
    {
        Task<Activity> GetActivityAsync(int id);
        Task<IEnumerable<Activity>> GetActivitiesAsync();
        Task<Activity> AddOrUpdateActivityAsync(Activity activity);
    }
}
