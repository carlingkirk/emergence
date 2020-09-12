using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;

namespace Emergence.Transform
{
    public class SynonymProcessor : ISynonymProcessor
    {
        private readonly ISynonymService _synonymService;
        private readonly IOriginService _originService;
        private readonly ITaxonService _taxonService;

        private Origin Origin;
        private List<Taxon> Taxons { get; set; }
        private List<Origin> Origins { get; set; }

        public SynonymProcessor(ISynonymService synonymService, IOriginService originService, ITaxonService taxonService)
        {
            _synonymService = synonymService;
            _originService = originService;
            _taxonService = taxonService;

            Taxons = new List<Taxon>();
            Origins = new List<Origin>();
        }

        public async Task InitializeOrigin(Origin origin)
        {
            Origin = await _originService.GetOriginAsync(origin.OriginId);
            if (Origin == null)
            {
                Origin = await _originService.AddOrUpdateOriginAsync(origin, null);
            }
        }

        public async Task InitializeTaxons()
        {
            var taxonResult = await _taxonService.GetTaxonsAsync();
            Taxons = taxonResult.ToList();
        }

        public async Task<Synonym> Process(Synonym synonym)
        {
            var taxon = Taxons.FirstOrDefault(t => t.Kingdom == synonym.Taxon.Kingdom
                                                && t.Subkingdom == synonym.Taxon.Subkingdom
                                                && t.Infrakingdom == synonym.Taxon.Infrakingdom
                                                && t.Phylum == synonym.Taxon.Phylum
                                                && t.Subphylum == synonym.Taxon.Subphylum
                                                && t.Class == synonym.Taxon.Class
                                                && t.Subclass == synonym.Taxon.Subclass
                                                && t.Superorder == synonym.Taxon.Superorder
                                                && t.Order == synonym.Taxon.Order
                                                && t.Family == synonym.Taxon.Family
                                                && t.Genus == synonym.Taxon.Genus
                                                && t.Species == synonym.Taxon.Species
                                                && t.Subspecies == synonym.Taxon.Subspecies
                                                && t.Variety == synonym.Taxon.Variety
                                                && t.Subvariety == synonym.Taxon.Subvariety
                                                && t.Form == synonym.Taxon.Form);

            if (taxon == null)
            {
                taxon = await _taxonService.AddOrUpdateTaxonAsync(synonym.Taxon);
                Taxons.Add(taxon);
            }

            var originResult = Origins.FirstOrDefault(o => o.ParentOrigin.OriginId == Origin.OriginId
                                                        && o.ExternalId == synonym.Origin.ExternalId
                                                        && o.AltExternalId == synonym.Origin.AltExternalId);
            if (originResult == null)
            {
                originResult = await _originService.GetOriginAsync(Origin.OriginId, synonym.Origin.ExternalId, synonym.Origin.AltExternalId);
                Origins.Add(originResult);
            }

            if (originResult == null)
            {
                originResult = await _originService.AddOrUpdateOriginAsync(synonym.Origin, null);
                Origins.Add(originResult);
            }

            synonym.Origin = originResult;

            var synonymResult = await _synonymService.GetSynonymAsync(s => s.Name == synonym.Name
                                                                        && s.Rank == synonym.Rank
                                                                        && s.TaxonId == synonym.TaxonId
                                                                        && s.OriginId == originResult.OriginId);

            if (synonymResult == null)
            {
                synonym.Origin = originResult;
                synonym.Taxon = taxon;
                synonymResult = await _synonymService.AddOrUpdateSynonymAsync(synonym);
            }

            return synonymResult;
        }

        public async Task<IEnumerable<Synonym>> Process(IEnumerable<Synonym> synonyms)
        {
            var synonymResults = new List<Synonym>();
            var newOrigins = new List<Origin>();
            var newSynonyms = new List<Synonym>();
            foreach (var synonym in synonyms)
            {
                var taxon = Taxons.FirstOrDefault(t => t.Kingdom == synonym.Taxon.Kingdom
                                                && t.Subkingdom == synonym.Taxon.Subkingdom
                                                && t.Infrakingdom == synonym.Taxon.Infrakingdom
                                                && t.Phylum == synonym.Taxon.Phylum
                                                && t.Subphylum == synonym.Taxon.Subphylum
                                                && t.Class == synonym.Taxon.Class
                                                && t.Subclass == synonym.Taxon.Subclass
                                                && t.Superorder == synonym.Taxon.Superorder
                                                && t.Order == synonym.Taxon.Order
                                                && t.Family == synonym.Taxon.Family
                                                && t.Genus == synonym.Taxon.Genus
                                                && t.Species == synonym.Taxon.Species
                                                && t.Subspecies == synonym.Taxon.Subspecies
                                                && t.Variety == synonym.Taxon.Variety
                                                && t.Subvariety == synonym.Taxon.Subvariety
                                                && t.Form == synonym.Taxon.Form);

                if (taxon == null)
                {
                    taxon = await _taxonService.AddOrUpdateTaxonAsync(synonym.Taxon);
                    Taxons.Add(taxon);
                }
                synonym.Taxon = taxon;

                // Do we already have the same origin in our insert list?
                var originResult = newOrigins.FirstOrDefault(o => o.ParentOrigin.OriginId == Origin.OriginId
                                                               && o.ExternalId == synonym.Origin.ExternalId
                                                               && o.AltExternalId == synonym.Origin.AltExternalId);
                if (originResult == null)
                {
                    // See if it already exists, if not, add it to the insert list
                    originResult = await _originService.GetOriginAsync(Origin.OriginId, synonym.Origin.ExternalId, synonym.Origin.AltExternalId);
                    if (originResult == null)
                    {
                        newOrigins.Add(synonym.Origin);
                    }
                    else
                    {
                        Origins.Add(originResult);
                    }
                }
            }

            if (newOrigins.Any())
            {
                newOrigins = (await _originService.AddOriginsAsync(newOrigins)).ToList();
                Origins.AddRange(newOrigins);
            }

            foreach (var synonym in synonyms)
            {
                var origin = Origins.FirstOrDefault(o => o.ParentOrigin.OriginId == Origin.OriginId
                                                      && o.ExternalId == synonym.Origin.ExternalId
                                                      && o.AltExternalId == synonym.Origin.AltExternalId);
                if (origin == null)
                {
                    origin = await _originService.GetOriginAsync(Origin.OriginId, synonym.Origin.ExternalId, synonym.Origin.AltExternalId);
                }

                synonym.Origin = origin;

                var synonymResult = await _synonymService.GetSynonymAsync(s => s.Name == synonym.Name
                                                                            && s.Rank == synonym.Rank
                                                                            && s.TaxonId == synonym.TaxonId
                                                                            && s.OriginId == synonym.Origin.OriginId);

                if (synonymResult == null)
                {
                    newSynonyms.Add(synonym);
                }
            }

            if (newSynonyms.Any())
            {
                newSynonyms = (await _synonymService.AddSynonymsAsync(newSynonyms)).ToList();
            }

            return newSynonyms;
        }
    }
}
