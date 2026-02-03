using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly ILaborRepository _laborRepo;
        private readonly IAdvancePaymentRepository _advanceRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            PaymentService paymentService,
            IAttendanceRepository attendanceRepo,
            ILaborRepository laborRepo,
            IAdvancePaymentRepository advanceRepo,
            IPaymentRepository paymentRepo,
            ISettingsRepository settingsRepo,
            ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _attendanceRepo = attendanceRepo;
            _laborRepo = laborRepo;
            _advanceRepo = advanceRepo;
            _paymentRepo = paymentRepo;
            _settingsRepo = settingsRepo;
            _logger = logger;
        }

        // ✅ GET: api/payments?from=&to=
        [HttpGet]
        public async Task<IActionResult> GetPayments(DateTime? from, DateTime? to)
        {
            var data = await _paymentRepo.GetWeeklyPaymentsAsync(from, to);
            return Ok(data);
        }

        // ✅ POST: api/payments/generate
        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePayments([FromBody] PaymentGenerateRequest request)
        {
            if (request.FromDate > request.ToDate)
                return BadRequest("Invalid date range");

            var attendances = await _attendanceRepo
                .GetAttendanceBetweenDatesAsync(request.FromDate, request.ToDate);

            var labors = _laborRepo.GetAll();
            var result = new List<WeeklyPayment>();

            foreach (var labor in labors)
            {
                var daysPresent = attendances.Count(a => a.LaborId == labor.Id);

                var dailyRate = labor.LaborType?.DailyWage > 0
                    ? labor.LaborType.DailyWage
                    : await _settingsRepo.GetDailyRateAsync();

                var gross = daysPresent * dailyRate;

                var advances = await _advanceRepo
                    .GetAdvancesBetweenDatesAsync(request.FromDate, request.ToDate);

                var totalAdvance = advances.Sum(x => x.Amount);
                var netSalary = gross - totalAdvance;

                var payment = new WeeklyPayment
                {
                    LaborId = labor.Id,
                    LaborName = labor.Name!,
                    LaborType = labor.LaborType?.LaborTypeName ?? "",
                    DaysPresent = daysPresent,
                    DailyRate = dailyRate,
                    AdvanceDeducted = totalAdvance,
                    NetSalary = netSalary,
                    PaymentMethod = request.PaymentMethod,
                    PaymentDate = DateTime.Now,
                    WeekStart = request.FromDate,
                    WeekEnd = request.ToDate
                };

                _paymentRepo.AddPayment(payment);
                result.Add(payment);
            }

            _paymentRepo.Save();
            _logger.LogInformation("Weekly payments generated");

            return Ok(result);
        }

        // ✅ GET: api/payments/{id}/slip
        [HttpGet("{id}/slip")]
        public IActionResult DownloadSlip(int id)
        {
            var payment = _paymentRepo.GetById(id);
            if (payment == null) return NotFound();

            using var ms = new MemoryStream();
            var doc = new PdfSharpCore.Pdf.PdfDocument();
            var page = doc.AddPage();
            var gfx = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
            var font = new PdfSharpCore.Drawing.XFont("Verdana", 14);

            gfx.DrawString("Payment Slip", font,
                PdfSharpCore.Drawing.XBrushes.Black,
                new PdfSharpCore.Drawing.XPoint(40, 40));

            gfx.DrawString($"Name: {payment.Labor.Name}", font,
                PdfSharpCore.Drawing.XBrushes.Black,
                new PdfSharpCore.Drawing.XPoint(40, 80));

            //gfx.DrawString($"Amount: ₹{payment.Labor.NetSalary}", font,
            //    PdfSharpCore.Drawing.XBrushes.Black,
            //    new PdfSharpCore.Drawing.XPoint(40, 120));

            doc.Save(ms, false);

            return File(ms.ToArray(), "application/pdf",
                $"PaymentSlip_{payment.Labor.Name}.pdf");

        }
    }
}
