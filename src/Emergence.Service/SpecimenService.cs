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
    public class SpecimenService : ISpecimenService
    {
        private readonly IRepository<Specimen> _specimenRepository;

        public SpecimenService(IRepository<Specimen> specimenRepository)
        {
            _specimenRepository = specimenRepository;
        }

        public async Task<Data.Shared.Models.Specimen> AddOrUpdateAsync(Data.Shared.Models.Specimen specimen, string userId)
        {
            var specimenResult = await _specimenRepository.AddOrUpdateAsync(s => s.Id == specimen.SpecimenId, specimen.AsStore());

            return specimenResult.AsModel();
        }

        public async Task<Data.Shared.Models.Specimen> GetSpecimenAsync(int specimenId, Data.Shared.Models.User user)
        {
            var specimenQuery = _specimenRepository.WhereWithIncludes(s => s.Id == specimenId, false,
                                                                      s => s.Include(s => s.InventoryItem)
                                                                              .Include(ii => ii.InventoryItem.Inventory)
                                                                              .Include(ii => ii.InventoryItem.Origin)
                                                                              .Include(s => s.Lifeform));
            specimenQuery = specimenQuery.CanViewContent(user);

            var specimen = await specimenQuery.FirstOrDefaultAsync();

            return specimen?.AsModel();
        }

        public async Task<FindResult<Data.Shared.Models.Specimen>> FindSpecimens(FindParams findParams, Data.Shared.Models.User user)
        {
            var specimenQuery = _specimenRepository.WhereWithIncludes(s => (findParams.SearchTextQuery == null ||
                                                                           EF.Functions.Like(s.InventoryItem.Name, findParams.SearchTextQuery) ||
                                                                           EF.Functions.Like(s.Lifeform.CommonName, findParams.SearchTextQuery) ||
                                                                           EF.Functions.Like(s.Lifeform.ScientificName, findParams.SearchTextQuery)),
                                                                           false,
                                                                           s => s.Include(s => s.InventoryItem)
                                                                                 .Include(s => s.InventoryItem.Inventory)
                                                                                 .Include(s => s.InventoryItem.Origin)
                                                                                 .Include(s => s.Lifeform));


            if (!string.IsNullOrEmpty(findParams.CreatedBy))
            {
                specimenQuery = specimenQuery.Where(s => s.CreatedBy == findParams.CreatedBy);
            }

            specimenQuery = specimenQuery.CanViewContent(user);

            specimenQuery = OrderBy(specimenQuery, findParams.SortBy, findParams.SortDirection);

            var count = specimenQuery.Count();
            var specimenResult = specimenQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var specimens = new List<Data.Shared.Models.Specimen>();
            await foreach (var specimen in specimenResult)
            {
                specimens.Add(specimen.AsModel());
            }
            return new FindResult<Data.Shared.Models.Specimen>
            {
                Results = specimens,
                Count = count
            };
        }

        public async Task RemoveSpecimenAsync(Data.Shared.Models.Specimen specimen) => await _specimenRepository.RemoveAsync(specimen.AsStore());

        private IQueryable<Specimen> CanViewContent(IQueryable<Specimen> specimenQuery, Data.Shared.Models.User user) =>
            specimenQuery = specimenQuery.Where(c => (c.InventoryItem.Visibility != Visibility.Hidden &&
                                                      c.InventoryItem.User.ProfileVisibility != Visibility.Hidden) ||
                                                     (c.InventoryItem.Visibility == Visibility.Contacts &&
                                                      c.InventoryItem.User.Contacts.Any(c => c.UserId == user.Id) &&
                                                      c.InventoryItem.User.InventoryItemVisibility != Visibility.Hidden) ||
                                                     (c.InventoryItem.User.InventoryItemVisibility == Visibility.Contacts &&
                                                      c.InventoryItem.User.Contacts.Any(c => c.UserId == user.Id)));

        private IQueryable<Specimen> OrderBy(IQueryable<Specimen> specimenQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return specimenQuery;
            }

            if (sortBy == null)
            {
                sortBy = "DateCreated";
            }

            var specimenSorts = new Dictionary<string, Expression<Func<Specimen, object>>>
            {
                { "ScientificName", s => s.Lifeform.ScientificName },
                { "CommonName", s => s.Lifeform.CommonName },
                { "Quantity", s => s.InventoryItem.Quantity },
                { "Stage", s => s.SpecimenStage },
                { "Status", s => s.InventoryItem.Status },
                { "DateAcquired", s => s.InventoryItem.DateAcquired },
                { "Origin", s => s.InventoryItem.Origin },
                { "DateCreated", s => s.DateCreated }
            };

            if (sortDirection == SortDirection.Descending)
            {
                specimenQuery = specimenQuery.WithOrder(p => p.OrderByDescending(specimenSorts[sortBy]));
            }
            else
            {
                specimenQuery = specimenQuery.WithOrder(p => p.OrderBy(specimenSorts[sortBy]));
            }

            return specimenQuery;
        }
    }
}
