using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Client.Pages.Activities;
using Emergence.Client.Pages.Origins;
using Emergence.Client.Pages.PlantInfos;
using Emergence.Client.Pages.Specimens;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public class ModalServiceClient : IModalServiceClient
    {
        private readonly IModalService _modalService;
        public ModalServiceClient(IModalService modalService)
        {
            _modalService = modalService;
        }

        public async Task<ModalResult> ShowSpecimenModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            modalParams.Add("IsModal", true);

            var specimenModal = _modalService.Show<SpecimenViewer>("Specimen", modalParams);
            return await specimenModal.Result;
        }

        public async Task<ModalResult> ShowSpecimenModal(Specimen specimen, bool isEditing = false)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Specimen", specimen);
            modalParams.Add("IsModal", true);
            modalParams.Add("IsEditing", isEditing);

            var specimenModal = _modalService.Show<SpecimenViewer>("Specimen", modalParams);
            return await specimenModal.Result;
        }

        public async Task<ModalResult> ShowOriginModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<OriginViewer>("Origin", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowOriginModal(Origin origin)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Origin", origin);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<OriginViewer>("Origin", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPlantInfoModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<PlantInfoViewer>("Plant Profile", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPlantInfoModal(PlantInfo plantInfo)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("PlantInfo", plantInfo);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<PlantInfoViewer>("Plant Profile", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<ActivityViewer>("Activity", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(Activity activity)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Activity", activity);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<ActivityViewer>("Activity", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(Specimen specimen)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", 0);
            modalParams.Add("IsModal", true);
            modalParams.Add("SelectedSpecimen", specimen);

            var modal = _modalService.Show<ActivityViewer>("Activity", modalParams);
            return await modal.Result;
        }
    }
}
