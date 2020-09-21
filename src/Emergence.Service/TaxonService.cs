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
using Emergence.Service.Extensions;
using Emergence.Service.Interfaces;

namespace Emergence.Service
{
    public class TaxonService : ITaxonService
    {
        private readonly IRepository<Taxon> _taxonRepository;
        public TaxonService(IRepository<Taxon> taxonRepository)
        {
            _taxonRepository = taxonRepository;
        }

        public async Task<Data.Shared.Models.Taxon> GetTaxonAsync(int id)
        {
            var taxon = await _taxonRepository.GetAsync(l => l.Id == id);
            return taxon?.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Taxon>> GetTaxonsAsync()
        {
            var taxonResult = _taxonRepository.GetSomeAsync(l => l.Id > 0);
            var taxons = new List<Data.Shared.Models.Taxon>();
            await foreach (var taxon in taxonResult)
            {
                taxons.Add(taxon.AsModel());
            }
            return taxons;
        }

        public async Task<IEnumerable<Data.Shared.Models.Taxon>> GetTaxonsAsync(Expression<Func<Taxon, bool>> predicate)
        {
            var taxonResult = _taxonRepository.GetSomeAsync(predicate);
            var taxons = new List<Data.Shared.Models.Taxon>();
            await foreach (var taxon in taxonResult)
            {
                taxons.Add(taxon.AsModel());
            }
            return taxons;
        }

        public async Task<Data.Shared.Models.Taxon> AddOrUpdateTaxonAsync(Data.Shared.Models.Taxon taxon)
        {
            if (taxon.TaxonId > 0)
            {
                taxon.DateModified = DateTime.UtcNow;
            }

            var taxonResult = await _taxonRepository.AddOrUpdateAsync(t => t.Id == taxon.TaxonId, taxon.AsStore());
            return taxonResult.AsModel();
        }

        public async Task<Data.Shared.Models.Taxon> GetTaxonAsync(string genus, string species, string subspecies, string variety, string subvariety, string form)
        {
            var taxon = await _taxonRepository.GetAsync(t => t.Genus == genus && t.Species == species &&
                                                             (subspecies == null || t.Subspecies == subspecies) &&
                                                             (variety == null || t.Variety == variety) &&
                                                             (subvariety == null || t.Subvariety == subvariety) &&
                                                             (form == null || t.Form == form));
            return taxon?.AsModel();
        }

        public async Task<FindResult<Data.Shared.Models.Taxon>> FindTaxons(FindParams<Data.Shared.Models.Taxon> findParams)
        {
            if (findParams.SearchText != null)
            {
                findParams.SearchText = "%" + findParams.SearchText + "%";
            }
            var taxonShape = findParams.Shape;

            var taxonQuery = _taxonRepository.Where(t => (taxonShape.Subkingdom == null || t.Subkingdom == taxonShape.Subkingdom) &&
                                                         (taxonShape.Infrakingdom == null || t.Infrakingdom == taxonShape.Infrakingdom) &&
                                                         (taxonShape.Phylum == null || t.Phylum == taxonShape.Phylum) &&
                                                         (taxonShape.Subphylum == null || t.Subphylum == taxonShape.Subphylum) &&
                                                         (taxonShape.Class == null || t.Class == taxonShape.Class) &&
                                                         (taxonShape.Superorder == null || t.Superorder == taxonShape.Superorder) &&
                                                         (taxonShape.Order == null || t.Order == taxonShape.Order) &&
                                                         (taxonShape.Suborder == null || t.Suborder == taxonShape.Suborder) &&
                                                         (taxonShape.Family == null || t.Family == taxonShape.Family) &&
                                                         (taxonShape.Genus == null || t.Genus == taxonShape.Genus) &&
                                                         (taxonShape.Species == null || t.Species == taxonShape.Species));

            if (string.IsNullOrEmpty(taxonShape.Subkingdom))
            {
                taxonQuery.DistinctBy(t => t.Infrakingdom);
            }
            else if (string.IsNullOrEmpty(taxonShape.Infrakingdom))
            {
                taxonQuery.DistinctBy(t => t.Phylum);
            }
            else if (string.IsNullOrEmpty(taxonShape.Phylum))
            {
                taxonQuery.DistinctBy(t => t.Subphylum);
            }
            else if (string.IsNullOrEmpty(taxonShape.Subphylum))
            {
                taxonQuery.DistinctBy(t => t.Class);
            }
            else if (string.IsNullOrEmpty(taxonShape.Class))
            {
                taxonQuery.DistinctBy(t => t.Superorder);
            }
            else if (string.IsNullOrEmpty(taxonShape.Superorder))
            {
                taxonQuery.DistinctBy(t => t.Order);
            }
            else if (string.IsNullOrEmpty(taxonShape.Order))
            {
                taxonQuery.DistinctBy(t => t.Suborder);
            }
            else if (string.IsNullOrEmpty(taxonShape.Suborder))
            {
                taxonQuery.DistinctBy(t => t.Family);
            }
            else if (string.IsNullOrEmpty(taxonShape.Family))
            {
                taxonQuery.DistinctBy(t => t.Genus);
            }
            else if (string.IsNullOrEmpty(taxonShape.Genus))
            {
                taxonQuery.DistinctBy(t => t.Species);
            }

            taxonQuery = OrderBy(taxonQuery, findParams.SortBy, findParams.SortDirection);

            var count = taxonQuery.Count();

            var taxonResult = taxonQuery.GetSomeAsync(skip: findParams.Skip, take: findParams.Take, track: false);

            var taxons = new List<Data.Shared.Models.Taxon>();
            await foreach (var taxon in taxonResult)
            {
                taxons.Add(taxon.AsModel());
            }

            return new FindResult<Data.Shared.Models.Taxon>
            {
                Count = count,
                Results = taxons
            };
        }

        private IQueryable<Taxon> OrderBy(IQueryable<Taxon> taxonQuery, string sortBy = null, SortDirection sortDirection = SortDirection.None)
        {
            if (sortDirection == SortDirection.None)
            {
                return taxonQuery;
            }

            if (sortBy == null)
            {
                sortBy = "DateCreated";
            }

            var taxonSorts = new Dictionary<string, Expression<Func<Taxon, object>>>
            {
                { "Subkingdom", t => t.Subkingdom },
                { "Infrakingdom", t => t.Infrakingdom },
                { "Phylum", t => t.Phylum },
                { "Subphylum", t => t.Subphylum },
                { "Class", t => t.Class },
                { "Superorder", t => t.Superorder },
                { "Order", t => t.Order },
                { "Suborder", t => t.Suborder },
                { "Family", t => t.Family },
                { "Genus", t => t.Genus },
                { "Species", t => t.Species },
                { "DateCreated", t => t.DateCreated }
            };

            if (sortDirection == SortDirection.Descending)
            {
                taxonQuery = taxonQuery.WithOrder(t => t.OrderByDescending(taxonSorts[sortBy]));
            }
            else
            {
                taxonQuery = taxonQuery.WithOrder(t => t.OrderBy(taxonSorts[sortBy]));
            }

            return taxonQuery;
        }
    }
}
