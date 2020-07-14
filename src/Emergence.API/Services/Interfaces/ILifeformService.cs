using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.API.Services.Interfaces
{
    public interface ILifeformService
    {
        Task<Lifeform> GetLifeformAsync(int id);
        Task<IEnumerable<Lifeform>> GetLifeformsAsync();
        Task<Lifeform> AddOrUpdateLifeformAsync(Lifeform lifeform);
        Task<IEnumerable<Lifeform>> FindLifeforms(string search, string userId, int skip = 0, int take = 10);
    }
}
