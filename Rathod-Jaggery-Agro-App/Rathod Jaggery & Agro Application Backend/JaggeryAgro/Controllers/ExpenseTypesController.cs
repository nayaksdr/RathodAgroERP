using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/expense-types")]
    public class ExpenseTypesController : ControllerBase
    {
        private readonly IExpenseTypeRepository _repo;
        private readonly ILogger<ExpenseTypesController> _logger;

        public ExpenseTypesController(
            IExpenseTypeRepository repo,
            ILogger<ExpenseTypesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // 🔹 GET: api/expense-types
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _repo.GetAllAsync();
            return Ok(list);
        }

        // 🔹 GET: api/expense-types/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // 🔹 POST: api/expense-types
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseType model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.AddAsync(model);

            _logger.LogInformation("Expense Type created");
            return Ok(new { message = "Expense Type created successfully" });
        }

        // 🔹 PUT: api/expense-types/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseType model)
        {
            if (id != model.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.UpdateAsync(model);

            _logger.LogInformation("Expense Type updated");
            return Ok(new { message = "Expense Type updated successfully" });
        }

        // 🔹 DELETE: api/expense-types/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);

            _logger.LogInformation("Expense Type deleted");
            return Ok(new { message = "Expense Type deleted successfully" });
        }
    }
}

