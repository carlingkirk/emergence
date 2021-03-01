using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Enums;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface ITaxonService
    {
        Task<Taxon> GetTaxonAsync(int id);
        Task<IEnumerable<Taxon>> GetTaxonsAsync(Expression<Func<Data.Shared.Stores.Taxon, bool>> predicate);
        Task<IEnumerable<Taxon>> GetTaxonsAsync();
        Task<Taxon> AddOrUpdateTaxonAsync(Taxon taxon);
        Task<Taxon> GetTaxonAsync(string genus, string species, string subspecies, string variety, string subvariety, string form);
        Task<FindResult<Taxon>> FindTaxons(FindParams<Taxon> findParams, TaxonRank rank);
        Task<IEnumerable<Taxon>> AddTaxonsAsync(IEnumerable<Taxon> taxons);
    }
}
