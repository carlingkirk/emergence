using System.Threading.Tasks;
using Emergence.Service.Search;

namespace Emergence.Transform
{
    public interface IElasticProcessor<T>
    {
        Task<BulkIndexResponse> Process(int startId, int endId);
        Task<BulkIndexResponse> ProcessSome();
    }
}
