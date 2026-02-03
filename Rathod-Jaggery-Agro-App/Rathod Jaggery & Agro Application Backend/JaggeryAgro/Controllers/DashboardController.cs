using JaggeryAgro.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        // GET: api/dashboard?days=14
        [HttpGet]
        public async Task<IActionResult> GetDashboard([FromQuery] int days = 14)
        {
            var data = await _service.GetDailyAsync(days);
            return Ok(data);
        }
    }
}
    
