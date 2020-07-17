using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data;
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
            return taxon.AsModel();
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

        public async Task<Data.Shared.Models.Taxon> AddOrUpdateTaxonAsync(Data.Shared.Models.Taxon taxon)
        {
            taxon.DateModified = DateTime.UtcNow;
            var taxonResult = await _taxonRepository.AddOrUpdateAsync(t => t.Id == taxon.TaxonId, taxon.AsStore());
            return taxonResult.AsModel();
        }

        public async Task<Data.Shared.Models.Taxon> GetTaxonAsync(string species, string variety, string form)
        {
            var taxon = await _taxonRepository.GetAsync(t => t.Species == species && t.Variety == variety && t.Form == form);
            return taxon.AsModel();
        }
    }
}
