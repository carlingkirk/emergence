using System.Threading.Tasks;
using Blazored.Modal;
using Blazored.Modal.Services;
using Emergence.Client.Pages.Activities;
using Emergence.Client.Pages.Messages;
using Emergence.Client.Pages.Origins;
using Emergence.Client.Pages.PlantInfos;
using Emergence.Client.Pages.Specimens;
using Emergence.Client.Pages.Users;
using Emergence.Client.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Common
{
    public class ModalServiceClient : IModalServiceClient
    {
        private readonly IModalService _modalService;
        private readonly ModalOptions DefaultModalOptions;
        public ModalServiceClient(IModalService modalService)
        {
            _modalService = modalService;
            DefaultModalOptions = new ModalOptions
            {
                ContentScrollable = true
            };
        }

        public async Task<ModalResult> ShowSpecimenModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            modalParams.Add("IsModal", true);

            var specimenModal = _modalService.Show<SpecimenViewer>("Specimen", modalParams, DefaultModalOptions);
            return await specimenModal.Result;
        }

        public async Task<ModalResult> ShowSpecimenModal(Specimen specimen, bool isEditing = false)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Specimen", specimen);
            modalParams.Add("IsModal", true);
            modalParams.Add("IsEditing", isEditing);

            var specimenModal = _modalService.Show<SpecimenViewer>("Specimen", modalParams, DefaultModalOptions);
            return await specimenModal.Result;
        }

        public async Task<ModalResult> ShowOriginModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<OriginViewer>("Origin", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowOriginModal(Origin origin, bool isEditing = false)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Origin", origin);
            modalParams.Add("IsModal", true);
            modalParams.Add("IsEditing", isEditing);

            var modal = _modalService.Show<OriginViewer>("Origin", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPlantInfoModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<PlantInfoViewer>("Plant Profile", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPlantInfoModal(PlantInfo plantInfo)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("PlantInfo", plantInfo);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<PlantInfoViewer>("Plant Profile", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(int id)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", id);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<ActivityViewer>("Activity", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(Activity activity)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Activity", activity);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<ActivityViewer>("Activity", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowActivityModal(Specimen specimen)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", 0);
            modalParams.Add("IsModal", true);
            modalParams.Add("SelectedSpecimen", specimen);

            var modal = _modalService.Show<ActivityViewer>("Activity", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowUserModal(UserSummary user)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Id", user.Id);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<ViewUser>("User", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowPhotoModal(Photo photo, string name)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Photo", photo);
            modalParams.Add("Name", name);

            var modal = _modalService.Show<PhotoViewer>("Photo", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowMessageModal(UserMessage message, bool isSent)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Message", message);
            modalParams.Add("IsModal", true);
            modalParams.Add("IsSent", isSent);

            var modal = _modalService.Show<MessageViewer>("Message", modalParams, DefaultModalOptions);
            return await modal.Result;
        }

        public async Task<ModalResult> ShowMessageModal(UserSummary recipient, string subject)
        {
            var modalParams = new ModalParameters();
            modalParams.Add("Recipient", recipient);
            modalParams.Add("Subject", subject);
            modalParams.Add("IsModal", true);

            var modal = _modalService.Show<MessageViewer>("Message", modalParams, DefaultModalOptions);
            return await modal.Result;
        }
    }
}
