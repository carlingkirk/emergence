using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Client.Pages;
using Emergence.Data.Shared.Models;

namespace Emergence.Client
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

            var specimenModal = _modalService.Show<EditSpecimen>("", modalParams);
            return await specimenModal.Result;
        }

        public async Task<ModalResult> ShowSpecimenModal(Specimen specimen)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("SpecimenParam", specimen);

            var specimenModal = _modalService.Show<EditSpecimen>("", modalParams);
            return await specimenModal.Result;
        }

        public async Task<ModalResult> ShowOriginModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);

            var modal = _modalService.Show<EditOrigin>("", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowOriginModal(Origin origin)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Origin", origin);

            var modal = _modalService.Show<EditOrigin>("", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPlantInfoModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);

            var modal = _modalService.Show<EditPlantInfo>("", modalParams);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPlantInfoModal(PlantInfo plantInfo)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("PlantInfo", plantInfo);

            var modal = _modalService.Show<EditPlantInfo>("", modalParams);
            return await modal.Result;
        }
    }
}
