using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JaggeryAgro.Web.Controllers
{
    [ApiController]
    [Route("api/jaggery-sale-share")]
    public class JaggerySaleShareController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ISplitwiseRepository _repo;
        private readonly IJaggerySaleRepository _jaggeryRepo;
        private readonly IWebHostEnvironment _env;

        public JaggerySaleShareController(
            ApplicationDbContext context,
            ISplitwiseRepository repo,
            IJaggerySaleRepository jaggeryRepo,
            IWebHostEnvironment env)
        {
            _context = context;
            _repo = repo;
            _jaggeryRepo = jaggeryRepo;
            _env = env;
        }

        // ===========================
        // 1️⃣ Dashboard / Index
        // ===========================
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var members = await _repo.GetMembersAsync();
            var expenses = await _repo.GetExpensesAsync();
            var jaggeryShares = await _jaggeryRepo.GetAllSharesAsync();
            var payments = await _jaggeryRepo.GetAllPaymentsAsync();

            var statusList = jaggeryShares.Select(s =>
            {
                var paidByMembers = payments
                    .Where(p => p.JaggerySaleId == s.JaggerySaleId && p.ToMemberId == s.MemberId)
                    .Sum(p => p.Amount);

                var actualPaid = s.PaidAmount + paidByMembers;
                var pending = Math.Max(s.ShareAmount - actualPaid, 0);

                return new
                {
                    s.Id,
                    s.JaggerySaleId,
                    s.Member.Name,
                    s.ShareAmount,
                    PaidAmount = actualPaid,
                    PendingAmount = pending,
                    Status = pending == 0 ? "Paid" : "Pending"
                };
            });

            return Ok(statusList);
        }

        // ===========================
        // 2️⃣ Create Sale Share
        // ===========================
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateJaggeryShareDto dto)
        {
            var sale = await _context.JaggerySales.FindAsync(dto.JaggerySaleId);
            if (sale == null) return BadRequest("Invalid Sale");

            decimal perMemberShare = sale.TotalAmount / dto.SplitMemberIds.Count;

            foreach (var memberId in dto.SplitMemberIds)
            {
                _context.JaggerySaleShares.Add(new JaggerySaleShare
                {
                    JaggerySaleId = sale.Id,
                    MemberId = memberId,
                    ShareAmount = perMemberShare,
                    PaidAmount = memberId == dto.PayingMemberId ? dto.PaidAmount : 0,
                    NetAmount = memberId == dto.PayingMemberId
                        ? dto.PaidAmount - perMemberShare
                        : -perMemberShare
                });
            }

            await _context.SaveChangesAsync();
            return Ok("Share created successfully");
        }

        // ===========================
        // 3️⃣ Record Payment
        // ===========================
        [HttpPost("record-payment")]
        public async Task<IActionResult> RecordPayment([FromForm] RecordPaymentDto dto)
        {
            var payment = new JaggerySalePayment
            {
                FromMemberId = dto.FromMemberId,
                ToMemberId = dto.ToMemberId,
                JaggerySaleId = dto.SaleId,
                Amount = dto.Amount,
                PaymentMode = dto.PaymentMode,
                PaymentDate = DateTime.Now
            };

            if (dto.ProofImage != null)
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads/jaggery");
                Directory.CreateDirectory(folder);

                string file = Guid.NewGuid() + Path.GetExtension(dto.ProofImage.FileName);
                string path = Path.Combine(folder, file);

                using var stream = new FileStream(path, FileMode.Create);
                await dto.ProofImage.CopyToAsync(stream);

                payment.ProofImage = "/uploads/jaggery/" + file;
            }

            _context.JaggerySalePayments.Add(payment);
            await _context.SaveChangesAsync();

            return Ok("Payment recorded");
        }

        // ===========================
        // 4️⃣ Helpers
        // ===========================
        [HttpGet("sales")]
        public async Task<IActionResult> GetSales()
            => Ok(await _context.JaggerySales
                .Select(s => new
                {
                    s.Id,
                    s.TotalAmount,
                    s.SaleDate
                }).ToListAsync());

        [HttpGet("members")]
        public async Task<IActionResult> GetMembers()
            => Ok(await _context.Members
                .Select(m => new { m.Id, m.Name })
                .ToListAsync());

        [HttpGet("dealers")]
        public async Task<IActionResult> GetDealers()
        => Ok(await _context.Dealers
            .Select(d => new { d.Id, d.Name })
            .ToListAsync());

    }
}
