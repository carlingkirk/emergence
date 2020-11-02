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
    public class OriginService : IOriginService
    {
        private readonly IRepository<Origin> _originRepository;
        private readonly ILocationService _locationService;
        public OriginService(IRepository<Origin> originRepository, ILocationService locationService)
        {
            _originRepository = originRepository;
            _locationService = locationService;
        }

        public async Task<Data.Shared.Models.Origin> GetOriginAsync(int id, Data.Shared.Models.User user)
        {
            var originQuery = _originRepository.WhereWithIncludes(o => o.Id == id, false,
                                                                  o => o.Include(o => o.ParentOrigin)
                                                                        .Include(o => o.Location));
            originQuery = originQuery.CanViewContent(user);

            var origin = await originQuery.FirstOrDefaultAsync();

            return origin?.AsModel();
        }

        public async Task<Data.Shared.Models.Origin> GetOriginAsync(int id)
        {
            var origin = await _originRepository.GetWithIncludesAsync(o => o.Id == id, false,
                                                                      o => o.Include(o => o.ParentOrigin)
                                                                            .Include(o => o.Location));
            return origin?.AsModel();
        }

        public async Task<Data.Shared.Models.Origin> AddOrUpdateOriginAsync(Data.Shared.Models.Origin origin, string userId)
        {
            origin.DateModified = DateTime.UtcNow;
            origin.CreatedBy = userId;

            if (origin.Location != null)
            {
                origin.Location = await _locationService.AddOrUpdateLocationAsync(origin.Location);
            }

            if (origin.ParentOrigin != null && origin.ParentOrigin.OriginId == 0)
            {
                var parentOrigin = await _originRepository.AddOrUpdateAsync(l => l.Id == origin.ParentOrigin.OriginId, origin.ParentOrigin.AsStore());
                origin.ParentOrigin = parentOrigin.AsModel();
            }

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

        public async Task<FindResult<Data.Shared.Models.Origin>> FindOrigins(FindParams findParams, Data.Shared.Models.User user)
        {
            var originQuery = _originRepository.WhereWithIncludes(o => findParams.SearchTextQuery == null ||
                                                                       EF.Functions.Like(o.Name, findParams.SearchTextQuery) ||
                                                                       EF.Functions.Like(o.Description, findParams.SearchTextQuery) ||
                                                                       EF.Functions.Like(o.Location.City, findParams.SearchTextQuery) ||
                                                                       EF.Functions.Like(o.Location.AddressLine1, findParams.SearchTextQuery) ||
                                                                       EF.Functions.Like(o.Location.StateOrProvince, findParams.SearchTextQuery),
                                                                       false,
                                                                  o => o.Include(o => o.Location).Include(o => o.ParentOrigin));

            originQuery = originQuery.CanViewContent(user);

            if (!string.IsNullOrEmpty(findParams.CreatedBy))
            {
                originQuery = originQuery.Where(s => s.CreatedBy == findParams.CreatedBy);
            }

            originQuery = OrderBy(originQuery, findParams.SortBy, findParams.SortDirection);

            var count = originQuery.Count();
            var originResult = originQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var origins = new List<Data.Shared.Models.Origin>();
            await foreach (var origin in originResult)
            {
                origins.Add(origin.AsModel());
            }

            return new FindResult<Data.Shared.Models.Origin>
            {
                Results = origins,
                Count = count
            };
        }

        public async Task RemoveOriginAsync(Data.Shared.Models.Origin origin)
        {
            var children = _originRepository.GetSomeAsync(o => o.ParentOriginId == origin.OriginId, track: true);
            var updateOrigins = new List<Origin>();

            await foreach (var childOrigin in children)
            {
                childOrigin.ParentOrigin = null;
                childOrigin.ParentOriginId = null;

                updateOrigins.Add(childOrigin);
            }

            await _originRepository.UpdateSomeAsync(updateOrigins);

            await _originRepository.RemoveAsync(origin.AsStore());
        }

        private IQueryable<Origin> OrderBy(IQueryable<Origin> originQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return originQuery;
            }

            if (sortBy == null)
            {
                sortBy = "DateCreated";
            }

            var originSorts = new Dictionary<string, Expression<Func<Origin, object>>>
            {
                { "Name", o => o.Name },
                { "Type", o => o.Type },
                { "Description", o => o.Description },
                { "ParentOrigin", o => o.ParentOrigin.Name },
                { "City", o => o.Location.City },
                { "Link", o => o.Uri },
                { "DateCreated", o => o.DateCreated }
            };

            if (sortDirection == SortDirection.Descending)
            {
                originQuery = originQuery.WithOrder(p => p.OrderByDescending(originSorts[sortBy]));
            }
            else
            {
                originQuery = originQuery.WithOrder(p => p.OrderBy(originSorts[sortBy]));
            }

            return originQuery;
        }

        public async Task<IEnumerable<Data.Shared.Models.Origin>> AddOriginsAsync(IEnumerable<Data.Shared.Models.Origin> origins)
        {
            var originsResult = await _originRepository.AddSomeAsync(origins.Select(o => o.AsStore()));
            return originsResult.Select(o => o.AsModel());
        }
    }
}
