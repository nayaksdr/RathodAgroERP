using JaggeryAgroManagementSystem.Data;
using JaggeryAgroManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgroManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LaborPaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LaborPaymentController(ApplicationDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> AddPayment(LaborPayment payment)
        {
            _context.LaborPayments.Add(payment);
            await _context.SaveChangesAsync();
            return Ok(payment);
        }

        [HttpGet("by-labor/{laborId}")]
        public async Task<IActionResult> GetPayments(int laborId) =>
            Ok(await _context.LaborPayments.Where(p => p.LaborId == laborId).ToListAsync());
    }

}
