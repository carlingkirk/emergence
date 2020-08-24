using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface ILifeformService
    {
        Task<Lifeform> GetLifeformAsync(int id);
        Task<Lifeform> GetLifeformByScientificNameAsync(string scientificName);
        Task<IEnumerable<Lifeform>> GetLifeformsAsync();
        Task<Lifeform> AddOrUpdateLifeformAsync(Lifeform lifeform);
        Task<IEnumerable<Lifeform>> FindLifeforms(FindParams findParams);
    }
}
