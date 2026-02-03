using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{    
    [ApiController]
    [Route("api/labors")]
    public class LaborsController : ControllerBase
    {
        private readonly ILaborRepository _repo;
        private readonly ILaborTypeRepository _laborTypeRepo;

        public LaborsController(
            ILaborRepository repo,
            ILaborTypeRepository laborTypeRepo)
        {
            _repo = repo;
            _laborTypeRepo = laborTypeRepo;
        }

        // ✅ GET: api/labors
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAllAsync());
        }

        // ✅ GET: api/labors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var labor = await _repo.GetByIdAsync(id);
            return labor == null ? NotFound() : Ok(labor);
        }

        // ✅ POST: api/labors
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Labor labor)
        {
            labor.Role = "Labor";
            labor.IsActive = true;
            labor.PasswordHash ??= "1234";

            await _repo.AddAsync(labor);
            return Ok(labor);
        }

        // ✅ PUT: api/labors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Labor labor)
        {
            if (id != labor.Id) return BadRequest();

            labor.Role ??= "Labor";
            labor.IsActive = true;
            labor.PasswordHash ??= "1234";

            await _repo.UpdateAsync(labor);
            return Ok(labor);
        }

        // ✅ DELETE: api/labors/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        // ✅ GET: api/labors/types
        [HttpGet("labor-types")]
        public async Task<IActionResult> GetLaborTypes()
        {
            return Ok(await _laborTypeRepo.GetAllAsync());
        }

        // ================= AUTH =================

        // ✅ POST: api/labors/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LaborLoginViewModel model)
        {
            var labor = await _repo.GetByMobileAsync(model.Mobile);
            if (labor == null || model.Password != labor.PasswordHash)
                return Unauthorized("Invalid credentials");

            return Ok(new
            {
                labor.Id,
                labor.Name,
                Role = "Labor",
                Token = "JWT_TOKEN_PLACEHOLDER"
            });
        }

        // ✅ GET: api/labors/dashboard/{laborId}
        [Authorize(Roles = "Labor")]
        [HttpGet("dashboard/{laborId}")]
        public async Task<IActionResult> Dashboard(int laborId)
        {
            return Ok(new
            {
                Attendance = await _repo.GetAttendanceAsync(laborId),
                Salaries = await _repo.GetWeeklySalariesAsync(laborId)
            });
        }
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            var labors = await _repo.GetAllLaborsAsync();

            // Filter for "गुळ तयार" or return all based on your business logic
            var result = labors.Select(l => new {
                id = l.Id,
                name = l.Name,
                type = l.LaborType?.LaborTypeName
            });

            return Ok(result);
        }
    }
}
