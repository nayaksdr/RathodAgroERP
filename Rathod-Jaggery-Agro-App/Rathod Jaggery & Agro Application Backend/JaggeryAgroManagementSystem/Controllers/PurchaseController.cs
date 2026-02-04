using JaggeryAgroManagementSystem.Data;
using JaggeryAgroManagementSystem.DTOs;
using JaggeryAgroManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgroManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PurchaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchases() =>
            Ok(await _context.Purchases.Include(p => p.Supplier).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> AddPurchase(PurchaseDto dto)
        {
            var purchase = new Purchase
            {
                SupplierId = dto.SupplierId,
                PurchaseDate = dto.PurchaseDate,
                QuantityInKg = dto.QuantityKg,                
                RatePerKg = dto.RatePerKg    

            };
            var total = purchase.TotalAmount;

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return Ok(purchase);
        }
    }

}
