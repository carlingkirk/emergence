using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListSpecimensComponent : ListComponent<Specimen>
    {
        public override async Task<FindResult<Specimen>> GetListAsync(FindParams findParams)
        {
            var result = await ApiClient.FindSpecimensAsync(findParams);
            return new FindResult<Specimen>
            {
                Results = result.Results,
                Count = result.Count
            };
        }

        protected async Task UpdateSpecimenAsync(Specimen specimen)
        {
            var result = await ModalServiceClient.ShowSpecimenModal(specimen);
            if (!result.Cancelled)
            {
                specimen = List.Where(s => s.SpecimenId == specimen.SpecimenId).First();
                specimen = result.Data as Specimen;
            }
        }

        protected async Task UpdateOriginAsync(Origin origin)
        {
            var result = await ModalServiceClient.ShowOriginModal(origin.OriginId);
            if (!result.Cancelled)
            {
                List.Where(s => s.InventoryItem?.Origin?.OriginId == origin.OriginId).ToList().ForEach(s =>
                {
                    s.InventoryItem.Origin = result.Data as Origin;
                });
            }
        }
    }
}
