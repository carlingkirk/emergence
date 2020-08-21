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

            var specimenModal = _modalService.Show<EditSpecimen>("Add Specimen", modalParams);
            return await specimenModal.Result;
        }

        public async Task<ModalResult> ShowSpecimenModal(Specimen specimen)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Specimen", specimen);

            var specimenModal = _modalService.Show<EditSpecimen>("Edit Specimen", modalParams);
            return await specimenModal.Result;
        }

        public async Task<ModalResult> ShowOriginModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);

            var modal = _modalService.Show<EditOrigin>("Add Origin", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowOriginModal(Origin origin)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Origin", origin);

            var modal = _modalService.Show<EditOrigin>("Edit Origin", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPlantInfoModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);

            var modal = _modalService.Show<EditPlantInfo>("Add Plant Info", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPlantInfoModal(PlantInfo plantInfo)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("PlantInfo", plantInfo);

            var modal = _modalService.Show<EditPlantInfo>("Edit Plant Info", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);

            var modal = _modalService.Show<EditActivity>("Add Activity", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(Activity activity)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Activity", activity);

            var modal = _modalService.Show<EditActivity>("Edit Activity", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(Specimen specimen)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", 0);
            modalParams.Add("SelectedSpecimen", specimen);
            var modal = _modalService.Show<EditActivity>("Add Activity", modalParams);
            return await modal.Result;
        }
    }
}
