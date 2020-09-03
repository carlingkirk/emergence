using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Transform.USDA
{
    public interface IUSDAProcessor
    {
        Task InitializeOrigin(Origin origin);
        Task InitializeLifeforms();
        Task InitializeTaxons();
        Task<PlantInfo> Process(PlantInfo plantInfo);
        Task<IEnumerable<PlantInfo>> Process(IEnumerable<PlantInfo> plantInfo);
    }
}