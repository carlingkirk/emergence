using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
using Emergence.Data.Extensions;
using Emergence.Data.Shared.Enums;
using Emergence.Data.Shared.Extensions;
using Emergence.Data.Shared.Stores;
using Emergence.Service.Interfaces;

namespace Emergence.Service
{
    public class SynonymService : ISynonymService
    {
        private readonly IRepository<Synonym> _synonymRepository;
        public SynonymService(IRepository<Synonym> synonymRepository)
        {
            _synonymRepository = synonymRepository;
        }

        public async Task<Data.Shared.Models.Synonym> GetSynonymAsync(Expression<Func<Synonym, bool>> predicate)
        {
            var synonym = await _synonymRepository.GetAsync(predicate, false);
            return synonym?.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Synonym>> GetSynonymsAsync(IEnumerable<int> ids)
        {
            var synonymResult = _synonymRepository.GetSomeAsync(l => ids.Any(i => i == l.Id));
            var synonyms = new List<Data.Shared.Models.Synonym>();
            await foreach (var synonym in synonymResult)
            {
                synonyms.Add(synonym.AsModel());
            }
            return synonyms;
        }

        public async Task<IEnumerable<Data.Shared.Models.Synonym>> GetSynonymsFromTaxonsAsync(IEnumerable<int> taxonIds)
        {
            var synonymResult = _synonymRepository.WhereWithIncludes(s => taxonIds.Any(i => i == s.TaxonId), false, s => s.Include(s => s.Taxon)).GetSomeAsync();
            var synonyms = new List<Data.Shared.Models.Synonym>();
            await foreach (var synonym in synonymResult)
            {
                synonyms.Add(synonym.AsModel());
            }
            return synonyms;
        }

        public async Task<IEnumerable<Data.Shared.Models.Synonym>> GetSynonymsByParentAsync(TaxonRank rank, string taxonName)
        {
            var childRank = rank.GetChildRank();
            var synonymQuery = _synonymRepository.WhereWithIncludes(s => s.Rank == childRank.ToString() &&
                                                                        ((rank == TaxonRank.Kingdom && s.Taxon.Kingdom == taxonName) ||
                                                                         (rank == TaxonRank.Subkingdom && s.Taxon.Subkingdom == taxonName) ||
                                                                         (rank == TaxonRank.Infrakingdom && s.Taxon.Infrakingdom == taxonName) ||
                                                                         (rank == TaxonRank.Phylum && s.Taxon.Phylum == taxonName) ||
                                                                         (rank == TaxonRank.Subphylum && s.Taxon.Subphylum == taxonName) ||
                                                                         (rank == TaxonRank.Class && s.Taxon.Class == taxonName) ||
                                                                         (rank == TaxonRank.Subclass && s.Taxon.Subclass == taxonName) ||
                                                                         (rank == TaxonRank.Superorder && s.Taxon.Superorder == taxonName) ||
                                                                         (rank == TaxonRank.Order && s.Taxon.Order == taxonName) ||
                                                                         (rank == TaxonRank.Suborder && s.Taxon.Suborder == taxonName) ||
                                                                         (rank == TaxonRank.Family && s.Taxon.Family == taxonName) ||
                                                                         (rank == TaxonRank.Subfamily && s.Taxon.Subfamily == taxonName) ||
                                                                         (rank == TaxonRank.Genus && s.Taxon.Genus == taxonName) ||
                                                                         (rank == TaxonRank.Species && s.Taxon.Species == taxonName) ||
                                                                         (rank == TaxonRank.Subspecies && s.Taxon.Subspecies == taxonName) ||
                                                                         (rank == TaxonRank.Variety && s.Taxon.Subvariety == taxonName) ||
                                                                         (rank == TaxonRank.Subkingdom && s.Taxon.Subkingdom == taxonName)),
                                                                         false,
                                                                    s => s.Include(s => s.Taxon));

            var synonymResult = synonymQuery.GetSomeAsync();
            var synonyms = new List<Data.Shared.Models.Synonym>();
            await foreach (var synonym in synonymResult)
            {
                synonyms.Add(synonym.AsModel());
            }
            return synonyms;
        }

        public async Task<IEnumerable<Data.Shared.Models.Synonym>> GetSynonymsAsync(Expression<Func<Synonym, bool>> predicate)
        {
            var synonymResult = _synonymRepository.GetSomeAsync(predicate);
            var synonyms = new List<Data.Shared.Models.Synonym>();
            await foreach (var synonym in synonymResult)
            {
                synonyms.Add(synonym.AsModel());
            }
            return synonyms;
        }

        public async Task<Data.Shared.Models.Synonym> AddOrUpdateSynonymAsync(Data.Shared.Models.Synonym synonym)
        {
            var synonymResult = await _synonymRepository.AddOrUpdateAsync(s => s.Id == synonym.SynonymId, synonym.AsStore());
            return synonymResult.AsModel();
        }

        public async Task<IEnumerable<Data.Shared.Models.Synonym>> AddSynonymsAsync(IEnumerable<Data.Shared.Models.Synonym> synonyms)
        {
            var result = await _synonymRepository.AddSomeAsync(synonyms.Select(s => s.AsStore()));
            return result.Select(l => l.AsModel());
        }
    }
}
