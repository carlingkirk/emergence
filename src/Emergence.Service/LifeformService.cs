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

        public async Task<IEnumerable<Data.Shared.Models.Lifeform>> FindLifeforms(FindParams findParams)
        {
            if (findParams.SearchText != null)
            {
                findParams.SearchText = "%" + findParams.SearchText + "%";
            }

            var lifeformQuery = _lifeformRepository.Where(l => findParams.SearchText == null ||
                                                                        EF.Functions.Like(l.CommonName, findParams.SearchText) ||
                                                                        EF.Functions.Like(l.ScientificName, findParams.SearchText));
            lifeformQuery = OrderBy(lifeformQuery, findParams.SortBy, findParams.SortDirection);

            var count = lifeformQuery.Count();
            var lifeformResult = lifeformQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var lifeforms = new List<Data.Shared.Models.Lifeform>();
            await foreach (var lifeform in lifeformResult)
            {
                lifeforms.Add(lifeform.AsModel());
            }
            return lifeforms;
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
