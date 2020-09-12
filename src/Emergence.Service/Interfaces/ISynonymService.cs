using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Service.Interfaces
{
    public interface ISynonymService
    {
        Task<Synonym> GetSynonymAsync(Expression<Func<Data.Shared.Stores.Synonym, bool>> predicate);
        Task<IEnumerable<Synonym>> GetSynonymsAsync(IEnumerable<int> ids);
        Task<IEnumerable<Synonym>> GetSynonymsAsync(Expression<Func<Data.Shared.Stores.Synonym, bool>> predicate);
        Task<Synonym> AddOrUpdateSynonymAsync(Synonym synonym);
        Task<IEnumerable<Synonym>> AddSynonymsAsync(IEnumerable<Synonym> synonyms);
    }
}
