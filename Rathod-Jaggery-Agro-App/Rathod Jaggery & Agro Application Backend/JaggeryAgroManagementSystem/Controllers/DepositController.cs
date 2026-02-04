using JaggeryAgroManagementSystem.Data;
using JaggeryAgroManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgroManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepositController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DepositController(ApplicationDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> AddDeposit(Deposit deposit)
        {
            _context.Deposits.Add(deposit);
            await _context.SaveChangesAsync();
            return Ok(deposit);
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<IActionResult> GetSupplierDeposits(int supplierId) =>
            Ok(await _context.Deposits.Where(d => d.SupplierId == supplierId).ToListAsync());

        [HttpGet("labor/{laborId}")]
        public async Task<IActionResult> GetLaborDeposits(int laborId) =>
            Ok(await _context.Deposits.Where(d => d.LaborId == laborId).ToListAsync());
    }
}
