using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/dealer-advances")]
    public class DealerAdvancesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IDealerAdvanceRepository _advRepo;
        private readonly ILogger<DealerAdvancesController> _logger;
        private readonly ApplicationDbContext _context;

        public DealerAdvancesController(
            IDealerAdvanceRepository advRepo,
            IWebHostEnvironment env,
            ILogger<DealerAdvancesController> logger,
            ApplicationDbContext context)
        {
            _advRepo = advRepo;
            _env = env;
            _logger = logger;
            _context = context;
        }

        // 🔹 GET: api/dealer-advances?dealerId=&from=&to=
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] DealerAdvanceFilterDto filter)
        {
            var list = await _advRepo.FilterAsync(filter.DealerId, filter.From, filter.To);

            var result = list.Select(a => new DealerAdvanceDto
            {
                Id = a.Id,
                DealerId = a.DealerId,
                DealerName = a.Dealer?.Name ?? "N/A", // Map name from included Dealer
                PaidByName = a.PaidBy?.Name ?? "N/A", // Map name from included PaidBy
                PaymentDate = a.AdvanceDate,
                Amount = a.Amount,
                PaymentMode = a.PaymentMode,
                ProofImage = a.ProofImage,
                Remarks = a.Note
            });

            return Ok(result);
        }
        [HttpGet("{dealerId:int}/advance")]
        public async Task<IActionResult> GetDealerAdvance(int dealerId)
        {
            var advance = await _context.DealerAdvances
                .Where(x => x.DealerId == dealerId).SumAsync(x => (decimal?)x.Amount) ?? 0;                

            return Ok(advance);
        }

        // 🔹 GET by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _advRepo.GetByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(new DealerAdvanceDto
            {
                Id = item.Id,
                DealerId = item.DealerId,
                PaidById = item.PaidById.Value,
                PaymentDate = item.AdvanceDate,
                Amount = item.Amount,
                PaymentMode = item.PaymentMode,
                ProofImage = item.ProofImage,
                Remarks = item.Note
            });
        }

        // 🔹 POST: Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DealerAdvanceDto dto, IFormFile? proofImage)
        {
            var model = new DealerAdvance
            {
                DealerId = dto.DealerId,
                PaidById = dto.PaidById,
                AdvanceDate = dto.PaymentDate,
                Amount = dto.Amount,
                PaymentMode = dto.PaymentMode,
                Note = dto.Remarks
            };

            if ((dto.PaymentMode == "UPI" || dto.PaymentMode == "Bank") && proofImage != null)
                model.ProofImage = await SaveFile(proofImage);

            await _advRepo.AddAsync(model);
            _logger.LogInformation("Dealer advance created");

            return Ok(new { message = "Dealer advance added successfully" });
        }

        // 🔹 PUT: Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] DealerAdvanceDto dto,
            IFormFile? proofImage)
        {
            if (id != dto.Id) return BadRequest();

            var model = await _advRepo.GetByIdAsync(id);
            if (model == null) return NotFound();

            model.DealerId = dto.DealerId;
            model.PaidById = dto.PaidById;
            model.AdvanceDate = dto.PaymentDate;
            model.Amount = dto.Amount;
            model.PaymentMode = dto.PaymentMode;
            model.Note = dto.Remarks;

            if ((dto.PaymentMode == "UPI" || dto.PaymentMode == "Bank") && proofImage != null)
                model.ProofImage = await SaveFile(proofImage);

            await _advRepo.UpdateAsync(model);
            _logger.LogInformation("Dealer advance updated");

            return Ok(new { message = "Dealer advance updated successfully" });
        }

        // 🔹 DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _advRepo.DeleteAsync(id);
            _logger.LogInformation("Dealer advance deleted");

            return Ok(new { message = "Dealer advance deleted" });
        }

        // 🔹 File upload helper
        private async Task<string> SaveFile(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", "salesAdvPay");
            Directory.CreateDirectory(folder);

            var name = $"Adv_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(folder, name);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/salesAdvPay/{name}";
        }
        
    }
}
