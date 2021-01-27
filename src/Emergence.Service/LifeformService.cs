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
using Emergence.Service.Search;
using Microsoft.EntityFrameworkCore;
using SearchModels = Emergence.Data.Shared.Search.Models;

namespace Emergence.Service
{
    public class LifeformService : ILifeformService
    {
        private readonly IRepository<Lifeform> _lifeformRepository;
        private readonly IIndex<SearchModels.Lifeform, Data.Shared.Models.Lifeform> _lifeformIndex;

        public LifeformService(IRepository<Lifeform> lifeformRepository, IIndex<SearchModels.Lifeform, Data.Shared.Models.Lifeform> lifeformIndex)
        {
            _lifeformRepository = lifeformRepository;
            _lifeformIndex = lifeformIndex;
        }

        public async Task<Data.Shared.Models.Lifeform> AddOrUpdateLifeformAsync(Data.Shared.Models.Lifeform lifeform)
        {
            var lifeformResult = await _lifeformRepository.AddOrUpdateAsync(l => l.Id == lifeform.LifeformId, lifeform.AsStore());
            return lifeformResult.AsModel();
        }

        public async Task<Data.Shared.Models.Lifeform> GetLifeformAsync(int id)
        {
            var lifeform = await _lifeformRepository.GetAsync(l => l.Id == id);
            return lifeform?.AsModel();
        }

        public async Task<Data.Shared.Models.Lifeform> GetLifeformByScientificNameAsync(string scientificName)
        {
            var lifeform = await _lifeformRepository.GetAsync(l => l.ScientificName == scientificName);
            return lifeform?.AsModel();
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

        public async Task<FindResult<Data.Shared.Models.Lifeform>> FindLifeforms(FindParams<Data.Shared.Models.Lifeform> findParams)
        {
            var lifeformSearch = await _lifeformIndex.SearchAsync(findParams, null);

            var lifeformIds = lifeformSearch.Documents.Select(p => p.Id).ToArray();
            var lifeformQuery = _lifeformRepository.Where(l => lifeformIds.Contains(l.Id), false);
            var lifeformResult = lifeformQuery.GetSomeAsync(track: false);

            var lifeforms = new List<Data.Shared.Models.Lifeform>();
            await foreach (var lifeform in lifeformResult)
            {
                lifeforms.Add(lifeform.AsModel());
            }

            return new FindResult<Data.Shared.Models.Lifeform>
            {
                Results = lifeforms,
                Count = lifeformSearch.Count
            };
        }

        private IQueryable<Lifeform> OrderBy(IQueryable<Lifeform> lifeformQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return lifeformQuery;
            }

            if (sortBy == null)
            {
                sortBy = "DateCreated";
            }

            var lifeformSorts = new Dictionary<string, Expression<Func<Lifeform, object>>>
            {
                { "ScientificName", l => l.ScientificName },
                { "CommonName", l => l.CommonName }
            };

            if (sortDirection == SortDirection.Descending)
            {
                lifeformQuery = lifeformQuery.WithOrder(p => p.OrderByDescending(lifeformSorts[sortBy]));
            }
            else
            {
                lifeformQuery = lifeformQuery.WithOrder(p => p.OrderBy(lifeformSorts[sortBy]));
            }

            return lifeformQuery;
        }
    }
}
