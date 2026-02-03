using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Member = JaggeryAgro.Core.Entities.Member;

namespace JaggeryAgro.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SplitwiseController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ISplitwiseRepository _repo;
        private readonly IExpenseTypeRepository _expenseTypeRepo;
        private readonly ILogger<SplitwiseController> _logger;

        public SplitwiseController(
            ISplitwiseRepository repo,
            IExpenseTypeRepository expenseTypeRepo,
            ILogger<SplitwiseController> logger,
            IWebHostEnvironment env)
        {
            _repo = repo;
            _expenseTypeRepo = expenseTypeRepo;
            _logger = logger;
            _env = env;
        }

        // ================= DASHBOARD =================

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var members = await _repo.GetMembersAsync();
            var expenses = await _repo.GetExpensesAsync();
            var balances = await _repo.GetBalancesAsync();
            var settlements = await _repo.GetSettlementsAsync();

            return Ok(new { members, expenses, balances, settlements });
        }

        // ================= MEMBERS =================

        [HttpGet("members")]
        public async Task<IActionResult> GetMembers() => Ok(await _repo.GetMembersAsync());

        [HttpPost("members")]
        public async Task<IActionResult> AddMember([FromBody] Member member)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _repo.AddMemberAsync(member);
            return Ok(new { message = "Member added successfully" });
        }

        // ================= EXPENSES =================

        [HttpGet("expenses")]
        public async Task<IActionResult> GetExpenses() => Ok(await _repo.GetExpensesAsync());

        // MERGED: One clear Post method for Expenses
        [HttpPost("expenses")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddExpense([FromForm] ExpenseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🔹 Validate Payment Mode Logic
            if (dto.PaymentMode == "Cash")
            {
                dto.ProofImage = null; // Ignore proof for cash
            }
            else if ((dto.PaymentMode == "UPI" || dto.PaymentMode == "Bank") && dto.ProofImage == null)
            {
                return BadRequest("Payment proof image is required for UPI/Bank transactions.");
            }

            string? imagePath = null;

            if (dto.ProofImage != null)
            {
                imagePath = await SaveExpenseImage(dto.ProofImage);
            }

            var expense = new Expense
            {
                PaidById = dto.PaidById,
                Amount = dto.Amount,
                ExpenseTypeId = dto.ExpenseTypeId,
                PaymentMode = dto.PaymentMode,
                ProofImage = imagePath,
                Date = DateTime.Now
            };

            await _repo.AddExpenseAsync(expense);

            return Ok(new
            {
                message = "Expense added successfully",
                id = expense.Id
            });
        }
        [HttpPut("expenses/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateExpense(int id, [FromForm] ExpenseUpdateDto dto)
        {
            var existing = await _repo.GetExpenseByIdAsync(id);
            if (existing == null)
                return NotFound("Expense not found.");

            // 🔹 Validate Payment Mode
            if (dto.PaymentMode == "Cash")
            {
                dto.ProofImage = null;
                existing.ProofImage = null; // remove old proof if exists
            }
            else if ((dto.PaymentMode == "UPI" || dto.PaymentMode == "Bank")
                    && dto.ProofImage == null
                    && string.IsNullOrEmpty(existing.ProofImage))
            {
                return BadRequest("Payment proof image is required for UPI/Bank transactions.");
            }

            // 🔹 Save new proof if provided
            if (dto.ProofImage != null)
            {
                existing.ProofImage = await SaveExpenseImage(dto.ProofImage);
            }

            existing.Amount = dto.Amount;
            existing.PaidById = dto.PaidById;
            existing.ExpenseTypeId = dto.ExpenseTypeId;
            existing.PaymentMode = dto.PaymentMode;

            await _repo.UpdateExpenseAsync(existing);

            return Ok(new { message = "Expense updated successfully" });
        }

        [HttpDelete("expenses/{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            await _repo.DeleteExpenseAsync(id);
            return Ok(new { message = "Deleted successfully" });
        }

        [HttpGet("expense-types")]
        public async Task<IActionResult> GetExpenseTypes()
        {
            var types = await _expenseTypeRepo.GetAllAsync();
            return Ok(types.Select(t => new { id = t.Id, name = t.Name }));
        }

        // ================= PAYMENTS =================

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PaymentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.FromMemberId == dto.ToMemberId)
                return BadRequest("From and To member cannot be same.");

            if (dto.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var payment = new SplitwisePayment
            {
                FromMemberId = dto.FromMemberId,
                ToMemberId = dto.ToMemberId,
                Amount = dto.Amount,
                PaymentDate = DateTime.Now
            };

            await _repo.AddPaymentAsync(payment);

            return Ok(new
            {
                message = "Payment recorded successfully",
                payment
            });
        }


        // ================= REPORTS =================

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel()
        {
            var balances = await _repo.GetBalancesAsync();
            var members = await _repo.GetMembersAsync();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Balances");
            ws.Cell(1, 1).Value = "Member";
            ws.Cell(1, 2).Value = "Balance";

            int row = 2;
            foreach (var kv in balances)
            {
                ws.Cell(row, 1).Value = members.FirstOrDefault(m => m.Id == kv.Key)?.Name ?? "Unknown";
                ws.Cell(row, 2).Value = kv.Value;
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Splitwise_Report.xlsx");
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf()
        {
            var balances = await _repo.GetBalancesAsync();
            var members = await _repo.GetMembersAsync();

            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            doc.Add(new Paragraph("Member Balances").SetBold().SetFontSize(18));
            foreach (var kv in balances)
            {
                string name = members.FirstOrDefault(m => m.Id == kv.Key)?.Name ?? "Unknown";
                doc.Add(new Paragraph($"{name}: ₹{kv.Value}"));
            }

            doc.Close();
            return File(stream.ToArray(), "application/pdf", "Splitwise_Report.pdf");
        }

        // ================= HELPERS =================

        private async Task<string> SaveExpenseImage(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", "expenses");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var safeFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(folder, safeFileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/expenses/{safeFileName}";
        }

    }
}