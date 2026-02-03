using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Web.Controllers.Api
{
    [ApiController]
    [Route("api/jaggery-sales")]
    public class JaggerySalesController : ControllerBase
    {
        private readonly IJaggerySaleRepository _saleRepo;
        private readonly IDealerRepository _dealerRepo;
        private readonly IDealerAdvanceRepository _advRepo;
        private readonly ILaborRepository _laborRepo;
        private readonly IWebHostEnvironment _env;
        private readonly ISplitwiseRepository _repo;
        public JaggerySalesController(
            IJaggerySaleRepository saleRepo,
            IDealerRepository dealerRepo,
            IDealerAdvanceRepository advRepo,
            ILaborRepository laborRepo,
            ISplitwiseRepository repo,
            IWebHostEnvironment env)
        {
            _saleRepo = saleRepo;
            _dealerRepo = dealerRepo;
            _advRepo = advRepo;
            _laborRepo = laborRepo;
            _repo = repo;
            _env = env;
        }

        // 1️⃣ GET: api/jaggery-sales (Filtered List)
        [HttpGet("all")]
        public async Task<IActionResult> GetAll(int? dealerId, DateTime? from, DateTime? to)
        {
            var sales = await _saleRepo.QueryAsync(dealerId, from, to);
            return Ok(sales);

        }
        [HttpGet("search")]
        public async Task<IActionResult> GetSales([FromQuery] string? searchDealer, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var sales = await _saleRepo.GetFilteredAsync(searchDealer, fromDate, toDate);
            return Ok(sales);
        }
        // 2️⃣ GET: api/jaggery-sales/{id} (Single Record)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sale = await _saleRepo.GetByIdAsync(id);
            if (sale == null) return NotFound();
            return Ok(sale);
        }

        // 3️⃣ POST: api/jaggery-sales (Create)
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateJaggerySaleDto dto, IFormFile? proofImage)
        {
            // Validation: Labor Type
            var labor = await _laborRepo.GetByIdAsync(dto.LaborId);
            if (labor?.LaborType == null || !labor.LaborType.LaborTypeName.Contains("गुळ तयार"))
                return BadRequest("Invalid Labor type. Only 'गुळ तयार' is allowed.");

            var sale = new JaggerySale
            {
                DealerId = dto.DealerId,
                LaborId = dto.LaborId,
                SaleDate = dto.SaleDate,
                QuantityInKg = dto.QuantityInKg,
                RatePerKg = dto.RatePerKg,
                PaymentMode = dto.PaymentMode,
                PaidById = dto.PaidById,
                TotalAmount = dto.QuantityInKg * dto.RatePerKg
            };

            // Advance Calculation
            var available = await GetAvailableAdvance(dto.DealerId);
            sale.AdvancePaid = Math.Min(dto.AdvancePaid > 0 ? dto.AdvancePaid : available, Math.Min(available, sale.TotalAmount));
            sale.RemainingAmount = sale.TotalAmount - sale.AdvancePaid;

            if (proofImage != null) sale.ProofImage = await SaveImage(proofImage);

            await _saleRepo.AddAsync(sale);
            return CreatedAtAction(nameof(GetById), new { id = sale.Id }, sale);
        }

        // 4️⃣ PUT: api/jaggery-sales/{id} (Update)
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateJaggerySaleDto dto, IFormFile? proofImage)
        {
            var existingSale = await _saleRepo.GetByIdAsync(id);
            if (existingSale == null) return NotFound();

            existingSale.DealerId = dto.DealerId;
            existingSale.LaborId = dto.LaborId;
            existingSale.QuantityInKg = dto.QuantityInKg;
            existingSale.RatePerKg = dto.RatePerKg;
            existingSale.TotalAmount = dto.QuantityInKg * dto.RatePerKg;

            // Recalculate Advance (Excluding current sale)
            var totalAdv = await _advRepo.GetTotalAdvanceByDealer(dto.DealerId);
            var usedOthers = await _saleRepo.GetTotalAdvanceAppliedByDealerExceptAsync(dto.DealerId, id);
            var available = totalAdv - usedOthers;

            existingSale.AdvancePaid = Math.Min(dto.AdvancePaid, Math.Min(available, existingSale.TotalAmount));
            existingSale.RemainingAmount = existingSale.TotalAmount - existingSale.AdvancePaid;

            if (proofImage != null) existingSale.ProofImage = await SaveImage(proofImage);

            await _saleRepo.UpdateAsync(existingSale);
            return NoContent();
        }

        // 5️⃣ DELETE: api/jaggery-sales/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sale = await _saleRepo.GetByIdAsync(id);
            if (sale == null) return NotFound();

            await _saleRepo.DeleteAsync(id);
            return Ok(new { message = "Sale deleted successfully" });
        }

        // 6️⃣ GET: api/jaggery-sales/dealer/{dealerId}/balance
        [HttpGet("dealer/{dealerId}/balance")]
        public async Task<IActionResult> GetDealerBalance(int dealerId)
        {
            var sales = await _saleRepo.GetByDealerAsync(dealerId);
            var totalAdvance = await _advRepo.GetTotalAdvanceByDealer(dealerId);

            return Ok(new
            {
                DealerId = dealerId,
                Sales = sales,
                TotalDealerAdvance = totalAdvance
            });
        }

        // --- Private Helpers ---

        private async Task<decimal> GetAvailableAdvance(int dealerId)
        {
            var total = await _advRepo.GetTotalAdvanceByDealer(dealerId);
            var used = await _saleRepo.GetTotalAdvanceAppliedByDealerAsync(dealerId, DateTime.Now);
            return total - used;
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "sales");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            string fileName = $"Sale_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            return $"/uploads/sales/{fileName}";
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetSales([FromQuery] int? dealerId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            try
            {
                var query = _saleRepo.GetAllQueryable();

                if (dealerId.HasValue)
                    query = query.Where(s => s.DealerId == dealerId);

                var sales = await query
                    .OrderByDescending(s => s.SaleDate)
                    .Select(s => new {
                        s.Id,
                        DealerName = s.Dealer != null ? s.Dealer.Name : "N/A",
                        s.TotalAmount,
                        s.SaleDate
                        // Add other fields as needed
                    }).ToListAsync();

                return Ok(sales);
            }
            catch (Exception ex)
            {
                // This will print the error to your console so you can see why it's failing
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }
        [HttpGet("dropdowns")]
        public async Task<IActionResult> GetDropdownData()
        {
            // Now these won't be null!
            var dealers = await _dealerRepo.GetAllAsync();
            var members = await _repo.GetMembersAsync();
            var labors = await _laborRepo.GetAllLaborsAsync();

            return Ok(new { dealers, members, labors });
        }
    }
}