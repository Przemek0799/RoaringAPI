using Microsoft.AspNetCore.Mvc;
using RoaringAPI.Interface;

namespace RoaringAPI.ControllersRoaring
{
    [ApiController]
    [Route("[controller]")]
    public class CompanySearchController : ControllerBase
    {
        private readonly IRoaringApiService _roaringApiService;

        public CompanySearchController(IRoaringApiService roaringApiService)
        {
            _roaringApiService = roaringApiService;
        }

        [HttpGet("searchByFreeText/{freeText}")]
        public async Task<IActionResult> SearchByFreeText(string freeText)
        {
            var result = await _roaringApiService.FetchCompanyByFreeTextAsync(freeText);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }
    }
}
