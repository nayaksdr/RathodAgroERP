using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Services;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/labor-type-rates")]
    public class LaborTypeRatesController : ControllerBase
    {
        private readonly ILaborTypeRateService _service;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<LaborTypeRatesController> _logger;

        public LaborTypeRatesController(
            ILaborTypeRateService service,
            ApplicationDbContext db,
            ILogger<LaborTypeRatesController> logger)
        {
            _service = service;
            _db = db;
            _logger = logger;
        }

        // ================= GET ALL RATES =================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllRatesAsync();

            // Ensure laborTypeName is populated from DB if missing
            foreach (var dto in data)
            {
                if (string.IsNullOrEmpty(dto.LaborTypeName))
                {
                    var laborType = await _db.LaborTypes
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == dto.LaborTypeId);

                    if (laborType != null)
                        dto.LaborTypeName = laborType.LaborTypeName;
                }

                ApplyPaymentTypeLogic(dto);
            }

            return Ok(data); // returns List<LaborTypeRateDto>
        }

        // ================= GET LABOR TYPES =================
        [HttpGet("labor-types")]
        public async Task<IActionResult> GetLaborTypes()
        {
            var list = await _db.LaborTypes
                .Select(x => new { x.Id, x.LaborTypeName })
                .ToListAsync();

            return Ok(list);
        }

        // ================= CREATE RATE =================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LaborTypeRateDto dto)
        {
            ApplyPaymentTypeLogic(dto);

            await _service.AddRateAsync(dto);
            _logger.LogInformation("Labor Type Rate created");

            return Ok(dto);
        }

        // ================= UPDATE RATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] LaborTypeRateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            ApplyPaymentTypeLogic(dto);

            await _service.UpdateRateAsync(dto);
            _logger.LogInformation("Labor Type Rate updated");

            return Ok(dto);
        }

        // ================= DELETE RATE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteRateAsync(id);
            _logger.LogInformation("Labor Type Rate deleted");

            return NoContent();
        }

        // ================= BUSINESS LOGIC =================
        private void ApplyPaymentTypeLogic(LaborTypeRateDto dto)
        {
            var laborType = _db.LaborTypes
                .FirstOrDefault(x => x.Id == dto.LaborTypeId);

            if (laborType == null) return;

            var name = laborType.LaborTypeName.ToLower();

            if (name.Contains("cane") || name.Contains("breaker") || name.Contains("ऊस"))
                dto.PaymentType = "दर टनावर आधारित (Cane Breaker)";
            else if (name.Contains("jaggery") || name.Contains("गुळ"))
                dto.PaymentType = "गुळ उत्पादनावर आधारित दर (Jaggery Maker)";
            else
                dto.PaymentType = "दैनिक दर (Regular Labor)";
        }
    }
}
