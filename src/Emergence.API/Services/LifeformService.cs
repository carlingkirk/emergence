using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;

namespace Emergence.API.Services
{
    public class LifeformService : ILifeformService
    {
        private readonly IRepository<Lifeform> _lifeformRepository;
        public LifeformService(IRepository<Lifeform> lifeformRepository)
        {
            _lifeformRepository = lifeformRepository;
        }
        public async Task<Data.Shared.Models.Lifeform> AddOrUpdateLifeformAsync(Data.Shared.Models.Lifeform lifeform)
        {
            var lifeformResult = await _lifeformRepository.AddOrUpdateAsync(l => l.Id == lifeform.LifeformId, lifeform.AsStore());
            return lifeformResult.AsModel();
        }

        public async Task<Data.Shared.Models.Lifeform> GetLifeformAsync(int id)
        {
            var lifeform = await _lifeformRepository.GetAsync(l => l.Id == id);
            return lifeform.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Lifeform>> GetLifeformsAsync()
        {
            var lifeformResult = _lifeformRepository.GetSomeAsync(l => l.Id > 0);
            var lifeforms = new List<Data.Shared.Models.Lifeform>();
            await foreach (var lifeform in lifeformResult)
            {
                lifeforms.Add(lifeform.AsModel());
            }
            return lifeforms;
        }
    }
}
