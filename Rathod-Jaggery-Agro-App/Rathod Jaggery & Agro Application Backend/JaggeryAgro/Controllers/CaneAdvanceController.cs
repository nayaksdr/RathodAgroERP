using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/cane-advances")]
    public class CaneAdvanceController : ControllerBase
    {
        private readonly ICaneAdvanceRepository _advanceRepo;
        private readonly IFarmerRepository _farmerRepo;
        private readonly ISplitwiseRepository _memberRepo;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<CaneAdvanceController> _logger;

        public CaneAdvanceController(
            ICaneAdvanceRepository advanceRepo,
            IFarmerRepository farmerRepo,
            ISplitwiseRepository memberRepo,
            IWebHostEnvironment env,
            ILogger<CaneAdvanceController> logger)
        {
            _advanceRepo = advanceRepo;
            _farmerRepo = farmerRepo;
            _memberRepo = memberRepo;
            _env = env;
            _logger = logger;
        }

        // 🔹 List
        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _advanceRepo.GetAllAsync());

        // 🔹 Dropdown Data
        [HttpGet("form-data")]
        public async Task<IActionResult> GetFormData()
        {
            return Ok(new
            {
                farmers = await _farmerRepo.GetAllAsync(),
                members = await _memberRepo.GetMembersAsync()
            });
        }

        // 🔹 Get By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var advance = await _advanceRepo.GetAsync(id);
            return advance == null ? NotFound() : Ok(advance);
        }

        // 🔹 Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CaneAdvance advance, IFormFile? proofImage)
        {
            if ((advance.PaymentMode == "UPI" || advance.PaymentMode == "Bank") && proofImage == null)
                return BadRequest("Proof image required for UPI/Bank");

            if (proofImage != null)
                advance.ProofImage = await SaveImage(proofImage);

            await _advanceRepo.AddAsync(advance);
            _logger.LogInformation("Cane Advance created");

            return Ok(advance);
        }

        // 🔹 Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] CaneAdvance advance, IFormFile? proofImage)
        {
            var existing = await _advanceRepo.GetAsync(id);
            if (existing == null) return NotFound();

            if ((advance.PaymentMode == "UPI" || advance.PaymentMode == "Bank") &&
                proofImage == null && string.IsNullOrEmpty(existing.ProofImage))
                return BadRequest("Proof image required");

            if (proofImage != null)
                existing.ProofImage = await SaveImage(proofImage);

            existing.AdvanceDate = advance.AdvanceDate;
            existing.Amount = advance.Amount;
            existing.FarmerId = advance.FarmerId;
            existing.MemberId = advance.MemberId;
            existing.PaymentMode = advance.PaymentMode;
            existing.Remarks = advance.Remarks;

            await _advanceRepo.UpdateAsync(existing);
            return Ok(existing);
        }

        // 🔹 Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var adv = await _advanceRepo.GetAsync(id);
            if (adv == null) return NotFound();

            await _advanceRepo.DeleteAsync(adv);
            return NoContent();
        }

        // 🔹 Image Upload Helper
        private async Task<string> SaveImage(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", "CaneAdvance");
            Directory.CreateDirectory(folder);

            var name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(folder, name);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/CaneAdvance/{name}";
        }
    }
}
