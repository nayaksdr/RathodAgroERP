using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.Services;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Web.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/cane-purchases")]
    public class CanePurchaseController : ControllerBase
    {
        private readonly ICanePurchaseService _service;
        private readonly IFarmerService _farmerService;
        private readonly ILaborRepository _laborRepo;
        private readonly ICanePurchaseRepository _purchaseRepo;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CanePurchaseController(
            ICanePurchaseService service,
            IFarmerService farmerService,
            ILaborRepository laborRepo,
            ICanePurchaseRepository purchaseRepo,
            ApplicationDbContext db,
            IWebHostEnvironment env)
        {
            _service = service;
            _farmerService = farmerService;
            _laborRepo = laborRepo;
            _purchaseRepo = purchaseRepo;
            _db = db;
            _env = env;
        }

        // 🔹 Summary list (Index)
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(int? farmerId)
        {
            var query = _db.CanePurchases
                .Include(x => x.Farmer)
                .Include(x => x.Payments)
                .AsQueryable();

            if (farmerId.HasValue)
                query = query.Where(x => x.FarmerId == farmerId);

            var result = await query
                .GroupBy(p => new { p.FarmerId, p.Farmer!.Name })
                .Select(g => new CanePurchaseSummaryDto
                {
                    FarmerId = g.Key.FarmerId,
                    FarmerName = g.Key.Name,
                    TotalTons = g.Sum(x => x.QuantityTon),
                    TotalAmount = g.Sum(x => x.QuantityTon * x.RatePerTon),
                    TotalPaid = g.Sum(x => x.Payments!.Sum(p => p.Amount)),
                    CaneWeightImagePath = g.FirstOrDefault(x => x.CaneWeightImagePath != null)!.CaneWeightImagePath
                })
                .ToListAsync();

            return Ok(result);
        }

        // 🔹 Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CanePurchase model, IFormFile? caneWeightImage)
        {
            var farmer = await _farmerService.GetAsync(model.FarmerId);
            if (farmer == null) return BadRequest("Farmer not found");

            model.FarmerName = farmer.Name;
            model.PaymentStatus = "Pending";
            model.TotalAmountSnapshot = model.QuantityTon * model.RatePerTon;

            if (caneWeightImage != null)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads/caneweight");
                Directory.CreateDirectory(folder);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(caneWeightImage.FileName)}";
                var path = Path.Combine(folder, fileName);

                using var fs = new FileStream(path, FileMode.Create);
                await caneWeightImage.CopyToAsync(fs);

                model.CaneWeightImagePath = $"/uploads/caneweight/{fileName}";
            }

            await _service.CreateAsync(model);
            return Ok(new { message = "Cane purchase created" });
        }

        // 🔹 Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}