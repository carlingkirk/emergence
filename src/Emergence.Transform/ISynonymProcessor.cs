using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Transform
{
    public interface ISynonymProcessor
    {
        Task InitializeOrigin(Origin origin);
        Task InitializeTaxons();
        Task<Synonym> Process(Synonym synonym);
        Task<IEnumerable<Synonym>> Process(IEnumerable<Synonym> synonyms);
    }
}
