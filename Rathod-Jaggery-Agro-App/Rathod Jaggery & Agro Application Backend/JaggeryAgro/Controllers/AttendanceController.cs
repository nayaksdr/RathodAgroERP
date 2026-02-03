using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _repo;
        private readonly ILaborRepository _laborRepo;
        private readonly ILogger<AttendanceController> _logger;

        public AttendanceController(
            IAttendanceRepository repo,
            ILaborRepository laborRepo,
            ILogger<AttendanceController> logger)
        {
            _repo = repo;
            _laborRepo = laborRepo;
            _logger = logger;
        }

        // 🔹 GET: All attendance
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.GetAllAsync());

        // 🔹 GET: Form Data (Labors)
        [HttpGet("form-data")]
        public async Task<IActionResult> GetFormData()
        {
            var labors = await _laborRepo.GetAllAsync();

            var result = labors.Select(l => new
            {
                l.Id,
                l.Name,
                Disabled = l.LaborType != null &&
                           (l.LaborType.LaborTypeName.Contains("ऊस तोड") ||
                            l.LaborType.LaborTypeName.Contains("गुळ तयार"))
            });

            return Ok(result);
        }

        // 🔹 GET: Attendance by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var attendance = await _repo.GetByIdAsync(id);
            return attendance == null ? NotFound() : Ok(attendance);
        }

        // 🔹 POST: Create Attendance
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Attendance attendance)
        {
            var labor = await _laborRepo.GetByIdAsync(attendance.LaborId);
            if (labor == null)
                return BadRequest("Invalid labor selected.");

            if (labor.LaborType != null &&
                (labor.LaborType.LaborTypeName.Contains("ऊस तोड") ||
                 labor.LaborType.LaborTypeName.Contains("गुळ तयार")))
                return BadRequest($"Attendance not required for {labor.LaborType.LaborTypeName}");

            await _repo.AddAsync(attendance);
            _logger.LogInformation("Attendance created for LaborId {LaborId}", attendance.LaborId);

            return Ok(attendance);
        }

        // 🔹 PUT: Update Attendance
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Attendance attendance)
        {
            if (id != attendance.Id)
                return BadRequest();

            await _repo.UpdateAsync(attendance);
            _logger.LogInformation("Attendance updated for LaborId {LaborId}", attendance.LaborId);

            return Ok(attendance);
        }

        // 🔹 DELETE: Attendance
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            _logger.LogInformation("Attendance deleted with Id {Id}", id);

            return NoContent();
        }
    }
}
