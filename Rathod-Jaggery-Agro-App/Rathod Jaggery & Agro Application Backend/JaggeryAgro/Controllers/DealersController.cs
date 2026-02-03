using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/dealers")]
    public class DealersController : ControllerBase
    {
        private readonly IDealerRepository _repo;
        private readonly ILogger<DealersController> _logger;
        private const int PageSize = 5;

        public DealersController(
            IDealerRepository repo,
            ILogger<DealersController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        // 🔹 GET: api/dealers?search=&page=1
        [HttpGet]
        public async Task<IActionResult> GetAll(
            string search = "",
            int page = 1)
        {
            var dealers = await _repo.GetAllAsync(search, page, PageSize);
            var total = await _repo.GetCountAsync(search);

            var result = new PagedResultDto<DealerDto>
            {
                Data = dealers.Select(d => new DealerDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Mobile = d.ContactNumber,
                    Address = d.Address
                }),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)PageSize),
                TotalCount = total
            };

            return Ok(result);
        }
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            var dealers = await _repo.GetDealerAsync();

            var result = dealers.Select(d => new
            {
                d.Id,
                d.Name
            });

            return Ok(result);
        }


        // 🔹 GET: api/dealers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var dealer = await _repo.GetByIdAsync(id);
            if (dealer == null)
                return NotFound();

            return Ok(new DealerDto
            {
                Id = dealer.Id,
                Name = dealer.Name,
                Mobile = dealer.ContactNumber,
                Address = dealer.Address
            });
        }

        // 🔹 POST: api/dealers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DealerDto dto)
        {
            var dealer = new Dealer
            {
                Name = dto.Name,
                ContactNumber = dto.Mobile,
                Address = dto.Address
            };

            await _repo.AddAsync(dealer);
            _logger.LogInformation("Dealer created successfully");

            return Ok(new { message = "Dealer created successfully" });
        }

        // 🔹 PUT: api/dealers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DealerDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var dealer = await _repo.GetByIdAsync(id);
            if (dealer == null)
                return NotFound();

            dealer.Name = dto.Name;
            dealer.ContactNumber = dto.Mobile;
            dealer.Address = dto.Address;

            await _repo.UpdateAsync(dealer);
            _logger.LogInformation("Dealer updated successfully");

            return Ok(new { message = "Dealer updated successfully" });
        }

        // 🔹 DELETE: api/dealers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeleteAsync(id);
            _logger.LogInformation("Dealer deleted successfully");

            return Ok(new { message = "Dealer deleted successfully" });
        }
    }
}
