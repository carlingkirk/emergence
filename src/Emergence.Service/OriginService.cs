using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Service
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
            return origin?.AsModel();
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
            origin.DateModified = DateTime.UtcNow;
            var originResult = await _originRepository.AddOrUpdateAsync(l => l.Id == origin.OriginId, origin.AsStore());
            return originResult.AsModel();
        }

        public async Task<Data.Shared.Models.Origin> GetOriginAsync(int parentOriginId, string externalId, string altExternalId)
        {
            var origin = await _originRepository.GetAsync(o => o.ParentOriginId == parentOriginId &&
                                                               o.ExternalId == externalId &&
                                                              (altExternalId == null || o.AltExternalId == altExternalId));
            return origin?.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Origin>> FindOrigins(string search, string userId, int skip = 0, int take = 10)
        {
            search = "%" + search + "%";
            var originResult = _originRepository.WhereWithIncludesAsync(o => o.UserId == userId &&
                                                                    (EF.Functions.Like(o.Name, search) ||
                                                                    EF.Functions.Like(o.Description, search) ||
                                                                    EF.Functions.Like(o.Location.City, search) ||
                                                                    EF.Functions.Like(o.Location.AddressLine1, search) ||
                                                                    EF.Functions.Like(o.Location.StateOrProvince, search)),
                                                                        o => o.Include(o => o.Location))
                                                    .WithOrder(o => o.OrderByDescending(o => o.DateCreated))
                                                    .GetSomeAsync(skip: skip, take: take, track: false);

            var origins = new List<Data.Shared.Models.Origin>();
            await foreach (var origin in originResult)
            {
                origins.Add(origin.AsModel());
            }

            return origins;
        }
    }
}
