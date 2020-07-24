using System.Collections.Generic;
using System.Threading.Tasks;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public interface IApiClient
    {
        Task<IEnumerable<Origin>> FindOrigins(string searchText);
        Task<IEnumerable<Lifeform>> FindLifeforms(string searchText);
        Task<Specimen> GetSpecimen(int id);
        Task<Specimen> PutSpecimen(Specimen specimen);
        Task<PlantInfo> GetPlantInfo(int id);
        Task<PlantInfo> PutPlantInfo(PlantInfo plantInfo);
    }
}
