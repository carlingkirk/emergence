using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Microsoft.EntityFrameworkCore;

namespace Emergence.API.Services
{
    public class OriginService : IOriginService
    {
        private readonly IRepository<Origin> _originRepository;
        public OriginService(IRepository<Origin> originRepository)
        {
            _originRepository = originRepository;
        }

        public async Task<Data.Shared.Models.Origin> GetOriginAsync(int id)
        {
            var origin = await _originRepository.GetAsync(l => l.Id == id);
            return origin.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Origin>> GetOriginsAsync()
        {
            var originResult = _originRepository.GetSomeAsync(l => l.Id > 0);
            var origins = new List<Data.Shared.Models.Origin>();
            await foreach (var origin in originResult)
            {
                origins.Add(origin.AsModel());
            }
            return origins;
        }

        public async Task<Data.Shared.Models.Origin> AddOrUpdateOriginAsync(Data.Shared.Models.Origin origin)
        {
            var originResult = await _originRepository.AddOrUpdateAsync(l => l.Id == origin.OriginId, origin.AsStore());
            return originResult.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Origin>> FindOrigins(string search, int skip = 0, int take = 10)
        {
            search = "%" + search + "%";
            var originResult = _originRepository.GetSomeAsync(o => EF.Functions.Like(o.Name, search) || EF.Functions.Like(o.Description, search),
                                                                  skip: skip, take: take, track: false);

            var origins = new List<Data.Shared.Models.Origin>();
            await foreach (var origin in originResult)
            {
                origins.Add(origin.AsModel());
            }

            return origins;
        }
    }
}
