using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/advance-payments")]
    public class AdvancePaymentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IAdvancePaymentRepository _repo;
        private readonly ILaborRepository _laborRepo;
        private readonly ILogger<AdvancePaymentController> _logger;

        public AdvancePaymentController(
            IAdvancePaymentRepository repo,
            ILaborRepository laborRepo,
            ApplicationDbContext context,
            ILogger<AdvancePaymentController> logger)
        {
            _repo = repo;
            _laborRepo = laborRepo;
            _logger = logger;
            _context = context;
        }

        // 🔹 GET: All advance payments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var payments = await _context.AdvancePayments
                    .Include(a => a.Labor)
                        .ThenInclude(l => l.LaborType)
                    .OrderByDescending(a => a.DateGiven)
                    .ToListAsync();

                var result = payments.Select(p => new {
                    id = p.Id,
                    laborId = p.LaborId,
                    LaborName = p.Labor.Name,
                    LaborType = p.Labor.LaborType.LaborTypeName,
                    amount = p.Amount,
                    dateGiven = p.DateGiven
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching advance payments");
                return StatusCode(500, ex.Message);
            }
        }




        // 🔹 GET: By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var payment = await _repo.GetByIdAsync(id);
            return payment == null ? NotFound() : Ok(payment);
        }
        // JaggeryAgro.Web.Controllers/AdvancePaymentController.cs

        [HttpGet("form-data")]
        public IActionResult GetFormData()
        {
            // Ensure we are fetching the data and projecting it to a clean object
            var labors = _laborRepo.GetAll()
                .Select(l => new { id = l.Id, name = l.Name }) // Use lowercase names for JSON compatibility
                .ToList();

            return Ok(labors);
        }

        // 🔹 POST: Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdvancePayment payment)
        {
            // 1. Handle default date
            if (payment.DateGiven == default)
                payment.DateGiven = DateTime.Now;

            // 2. FETCH the single labor record including its LaborType details
            // Using the specific method from your ILaborRepository
            var labor = await _laborRepo.GetByIdWithLaborTypeAsync(payment.LaborId);

            // 3. Check if labor exists
            if (labor == null)
            {
                return BadRequest("Invalid Labor ID: The labor record does not exist.");
            }

            // 4. Auto-assign the LaborType name
            // Assuming your Labor entity has a Navigation Property 'LaborType' 
            // which contains a property like 'TypeName' or 'Name'
           

            // 5. Save the payment using your AdvancePayment repository
            await _repo.AddAsync(payment);

            _logger.LogInformation("Labor advance payment created for Labor ID: {LaborId}", payment.LaborId);

            return Ok(payment);
        }

        // 🔹 PUT: Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AdvancePayment payment)
        {
            if (id != payment.Id)
                return BadRequest();

            if (payment.DateGiven == default)
                payment.DateGiven = DateTime.Now;

            await _repo.UpdateAsync(payment);

            _logger.LogInformation("Labor advance payment updated.");

            return Ok(payment);
        }

        // 🔹 DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);

            _logger.LogInformation("Labor advance payment deleted.");

            return NoContent();
        }
    }
}
