using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/labor-types")]
    public class LaborTypesController : ControllerBase
    {
        private readonly ILaborTypeRepository _repo;
        private readonly ILogger<LaborTypesController> _logger;

        public LaborTypesController(
            ILaborTypeRepository repo,
            ILogger<LaborTypesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // ✅ GET: api/labor-types
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetAllAsync();
            return Ok(data);
        }

        // ✅ GET: api/labor-types/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // ✅ POST: api/labor-types
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LaborType type)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.AddAsync(type);
            _logger.LogInformation("Labor Type created");

            return Ok(type);
        }

        // ✅ PUT: api/labor-types/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LaborType type)
        {
            if (id != type.Id)
                return BadRequest("Id mismatch");

            await _repo.UpdateAsync(type);
            _logger.LogInformation("Labor Type updated");

            return Ok(type);
        }

        // ✅ DELETE: api/labor-types/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            _logger.LogInformation("Labor Type deleted");

            return NoContent();
        }
    }
}
