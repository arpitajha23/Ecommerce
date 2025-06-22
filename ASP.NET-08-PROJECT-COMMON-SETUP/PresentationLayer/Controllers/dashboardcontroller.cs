using ApplicationLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Data;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class dashboardcontroller : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public dashboardcontroller(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("category-tree")]
        public async Task<IActionResult> GetCategoryTree()
        {
            var result = await _dashboardService.GetCategoryTreeAsync();
            return Ok(result);
        }


    }
}
