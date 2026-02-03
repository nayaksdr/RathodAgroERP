using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/deposits")]
    public class DepositsController : ControllerBase
    {
        private readonly IDepositRepository _repo;
        private readonly ILogger<DepositsController> _logger;

        public DepositsController(
            IDepositRepository repo,
            ILogger<DepositsController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // 🔹 GET: api/deposits
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var deposits = await _repo.GetAllAsync();
            return Ok(deposits);
        }

        // 🔹 GET: api/deposits/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var deposit = await _repo.GetByIdAsync(id);
            if (deposit == null)
                return NotFound();

            return Ok(deposit);
        }

        // 🔹 POST: api/deposits
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Deposit deposit)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.AddAsync(deposit);

            _logger.LogInformation("Deposit created successfully");
            return Ok(new { message = "Deposit created successfully" });
        }

        // 🔹 PUT: api/deposits/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Deposit deposit)
        {
            if (id != deposit.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.UpdateAsync(deposit);

            _logger.LogInformation("Deposit updated successfully");
            return Ok(new { message = "Deposit updated successfully" });
        }

        // 🔹 DELETE: api/deposits/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);

            _logger.LogInformation("Deposit deleted successfully");
            return Ok(new { message = "Deposit deleted successfully" });
        }
    }
}