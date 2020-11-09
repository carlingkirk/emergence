using System.Threading.Tasks;
using Blazored.Modal.Services;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public interface IModalServiceClient
    {
        Task<ModalResult> ShowSpecimenModal(int id);
        Task<ModalResult> ShowSpecimenModal(Specimen specimen, bool isEditing = false);
        Task<ModalResult> ShowOriginModal(int id);
        Task<ModalResult> ShowOriginModal(Origin origin, bool isEditing = false);
        Task<ModalResult> ShowPlantInfoModal(int id);
        Task<ModalResult> ShowPlantInfoModal(PlantInfo plantInfo);
        Task<ModalResult> ShowActivityModal(int id);
        Task<ModalResult> ShowActivityModal(Activity activity);
        Task<ModalResult> ShowActivityModal(Specimen specimen);
        Task<ModalResult> ShowUserModal(UserSummary user);
    }
}
