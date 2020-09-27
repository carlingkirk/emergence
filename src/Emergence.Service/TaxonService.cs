using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Enums;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
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

        public async Task<FindResult<Data.Shared.Models.Taxon>> FindTaxons(FindParams<Data.Shared.Models.Taxon> findParams, TaxonRank rank)
        {
            if (findParams.SearchText != null)
            {
                findParams.SearchText = "%" + findParams.SearchText + "%";
            }
            var taxonShape = findParams.Shape;

            var taxonQuery = _taxonRepository.WhereWithIncludes(t => t.Kingdom != null && (taxonShape == null ||
                                                        ((taxonShape.Kingdom == null || t.Kingdom == taxonShape.Kingdom) &&
                                                         (taxonShape.Subkingdom == null || t.Subkingdom == taxonShape.Subkingdom) &&
                                                         (taxonShape.Infrakingdom == null || t.Infrakingdom == taxonShape.Infrakingdom) &&
                                                         (taxonShape.Phylum == null || t.Phylum == taxonShape.Phylum) &&
                                                         (taxonShape.Subphylum == null || t.Subphylum == taxonShape.Subphylum) &&
                                                         (taxonShape.Class == null || t.Class == taxonShape.Class) &&
                                                         (taxonShape.Superorder == null || t.Superorder == taxonShape.Superorder) &&
                                                         (taxonShape.Order == null || t.Order == taxonShape.Order) &&
                                                         (taxonShape.Suborder == null || t.Suborder == taxonShape.Suborder) &&
                                                         (taxonShape.Family == null || t.Family == taxonShape.Family) &&
                                                         (taxonShape.Genus == null || t.Genus == taxonShape.Genus) &&
                                                         (taxonShape.Species == null || t.Species == taxonShape.Species))));

            if (taxonShape == null || rank == TaxonRank.Kingdom)
            {
                taxonQuery = taxonQuery.Where(t => taxonShape == null || taxonShape.Kingdom == null || t.Kingdom == taxonShape.Kingdom)
                                       .Select(t => new Taxon { Kingdom = t.Kingdom }).Distinct();
            }
            else
            {
                switch (rank)
                {
                    case TaxonRank.Species:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Species = t.Species }).Distinct().OrderBy(t => t.Species);
                        break;
                    case TaxonRank.Genus:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Genus = t.Genus }).Distinct().OrderBy(t => t.Genus);
                        break;
                    case TaxonRank.Subfamily:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Subfamily = t.Subfamily }).Distinct().OrderBy(t => t.Subfamily);
                        break;
                    case TaxonRank.Family:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Family = t.Family }).Distinct().OrderBy(t => t.Family);
                        break;
                    case TaxonRank.Suborder:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Suborder = t.Suborder }).Distinct().OrderBy(t => t.Suborder);
                        break;
                    case TaxonRank.Order:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Order = t.Order }).Distinct().OrderBy(t => t.Order);
                        break;
                    case TaxonRank.Superorder:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Superorder = t.Superorder }).Distinct().OrderBy(t => t.Superorder);
                        break;
                    case TaxonRank.Subclass:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Subclass = t.Subclass }).Distinct().OrderBy(t => t.Subclass);
                        break;
                    case TaxonRank.Class:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Class = t.Class }).Distinct().OrderBy(t => t.Class);
                        break;
                    case TaxonRank.Subphylum:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Subphylum = t.Subphylum }).Distinct().OrderBy(t => t.Subphylum);
                        break;
                    case TaxonRank.Phylum:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Phylum = t.Phylum }).Distinct().OrderBy(t => t.Phylum);
                        break;
                    case TaxonRank.Infrakingdom:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Infrakingdom = t.Infrakingdom }).Distinct().OrderBy(t => t.Infrakingdom);
                        break;
                    case TaxonRank.Subkingdom:
                        taxonQuery = taxonQuery.Select(t => new Taxon { Subkingdom = t.Subkingdom }).Distinct().OrderBy(t => t.Subkingdom);
                        break;
                }
            }

            var result = taxonQuery.GetSome().ToList();
            var count = result.Count();

            var taxonResult = result.Skip(findParams.Skip).Take(findParams.Take);

            var taxons = new List<Data.Shared.Models.Taxon>();
            foreach (var taxon in taxonResult)
            {
                taxons.Add(taxon.AsModel());
            }

            return await Task.FromResult(new FindResult<Data.Shared.Models.Taxon>
            {
                Count = count,
                Results = taxons
            });
        }
    }

    internal class TaxonComparer : IEqualityComparer<Taxon>
    {
        private TaxonRank Rank { get; set; }
        public TaxonComparer(TaxonRank rank)
        {
            Rank = rank;
        }
        public bool Equals(Taxon x, Taxon y)
        {
            switch (Rank)
            {
                case TaxonRank.Kingdom:
                    return string.Equals(x.Kingdom, y.Kingdom, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        public int GetHashCode(Taxon taxon)
        {
            switch (Rank)
            {
                case TaxonRank.Kingdom:
                    return taxon.Kingdom.GetHashCode();
            }

            return taxon.GetHashCode();
        }
    }
}
