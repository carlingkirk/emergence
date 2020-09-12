using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data;
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
