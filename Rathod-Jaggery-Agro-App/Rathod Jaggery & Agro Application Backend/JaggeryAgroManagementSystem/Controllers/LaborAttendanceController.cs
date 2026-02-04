using JaggeryAgroManagementSystem.Data;
using JaggeryAgroManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgroManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LaborAttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LaborAttendanceController(ApplicationDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> MarkAttendance(LaborAttendance attendance)
        {
            _context.LaborAttendances.Add(attendance);
            await _context.SaveChangesAsync();
            return Ok(attendance);
        }

        [HttpGet("by-labor/{laborId}")]
        public async Task<IActionResult> GetByLabor(int laborId) =>
            Ok(await _context.LaborAttendances.Where(a => a.LaborId == laborId).ToListAsync());
    }

}
