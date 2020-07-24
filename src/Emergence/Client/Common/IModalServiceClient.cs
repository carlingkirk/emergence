using System.Threading.Tasks;
using Blazored.Modal.Services;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public interface IModalServiceClient
    {
        Task<ModalResult> ShowSpecimenModal(int id);
        Task<ModalResult> ShowSpecimenModal(Specimen specimen);
        Task<ModalResult> ShowOriginModal(int id);
        Task<ModalResult> ShowOriginModal(Origin origin);
        Task<ModalResult> ShowPlantInfoModal(int id);
        Task<ModalResult> ShowPlantInfoModal(PlantInfo plantInfo);
    }
}