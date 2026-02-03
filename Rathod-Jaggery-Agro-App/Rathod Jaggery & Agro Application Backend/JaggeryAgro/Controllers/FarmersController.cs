using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{

    [ApiController]
    [Route("api/farmers")]
    public class FarmersController : ControllerBase
    {
        private readonly IFarmerService _service;
        private readonly ILogger<FarmersController> _logger;

        public FarmersController(
            IFarmerService service,
            ILogger<FarmersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // 🔹 GET: api/farmers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.ListAsync();
            return Ok(list);
        }

        // 🔹 GET: api/farmers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var farmer = await _service.GetAsync(id);
            if (farmer == null)
                return NotFound();

            return Ok(farmer);
        }

        // 🔹 POST: api/farmers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Farmer farmer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(farmer.AadhaarNumber))
                farmer.AadhaarNumber = "NA";

            await _service.CreateAsync(farmer);

            _logger.LogInformation("Farmer created successfully");
            return Ok(new { message = "Farmer created successfully" });
        }

        // 🔹 PUT: api/farmers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Farmer farmer)
        {
            if (id != farmer.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(farmer.AadhaarNumber))
                farmer.AadhaarNumber = "NA";

            await _service.UpdateAsync(farmer);

            _logger.LogInformation("Farmer updated successfully");
            return Ok(new { message = "Farmer updated successfully" });
        }

        // 🔹 DELETE: api/farmers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Farmer deleted successfully");
            return Ok(new { message = "Farmer deleted successfully" });
        }
    }
}
