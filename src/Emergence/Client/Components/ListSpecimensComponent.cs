using System.Linq;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Models;

namespace Emergence.Client.Components
{
    public class ListSpecimensComponent : ListComponent<Specimen>
    {
        public override async Task<FindResult<Specimen>> GetListAsync(string searchText, int? skip = 0, int? take = 10, string sortBy = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            var result = await ApiClient.FindSpecimensAsync(SearchText, skip, Take, SortBy, SortDirection);
            return new FindResult<Specimen>
            {
                Results = result.Results,
                Count = result.Count
            };
        }

        protected async Task UpdateSpecimenAsync(Specimen specimen)
        {
            var result = await ModalServiceClient.ShowSpecimenModal(specimen);
            specimen = List.Where(s => s.SpecimenId == specimen.SpecimenId).First();
            specimen = result.Data as Specimen;
        }
    }
}
