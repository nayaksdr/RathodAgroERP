using JaggeryAgroManagementSystem.Data;
using JaggeryAgroManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgroManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LaborController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LaborController(ApplicationDbContext context) => _context = context;

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.Labors.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Labor labor)
        {
            _context.Labors.Add(labor);
            await _context.SaveChangesAsync();
            return Ok(labor);
        }
    }
}
