using System.Threading.Tasks;
using Emergence.API.Services.Interfaces;
using Emergence.Data.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecimenController : ControllerBase
    {
        private readonly ISpecimenService _specimenService;
        public SpecimenController(ISpecimenService specimenService)
        {
            _specimenService = specimenService;
        }

        [HttpGet]
        public async Task<Specimen> Get(int id) => await _specimenService.GetSpecimenAsync(id);

        [HttpPut]

        public async Task<Specimen> Put(Specimen specimen) => await _specimenService.AddOrUpdateAsync(specimen);
    }
}
