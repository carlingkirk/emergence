using System.Threading.Tasks;
using Emergence.Data.Shared;
using Emergence.Data.Shared.Enums;
using Emergence.Data.Shared.Models;
using Emergence.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emergence.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxonController : ControllerBase
    {
        private readonly ITaxonService _taxonService;

        public TaxonController(ITaxonService taxonService)
        {
            _taxonService = taxonService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Taxon> Get(int id) => await _taxonService.GetTaxonAsync(id);

        [AllowAnonymous]
        [HttpPost]
        [Route("Find")]
        public async Task<FindResult<Taxon>> FindTaxons(FindParams<Taxon> findParams, TaxonRank rank)
        {
            var result = await _taxonService.FindTaxons(findParams, rank);

            return result;
        }
    }
}
