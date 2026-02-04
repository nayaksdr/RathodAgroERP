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
    public class SupplierController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers() =>
            Ok(await _context.Suppliers.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> AddSupplier(SupplierDto dto)
        {
            var supplier = new Supplier
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address
            };
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return Ok(supplier);
        }
    }

}
