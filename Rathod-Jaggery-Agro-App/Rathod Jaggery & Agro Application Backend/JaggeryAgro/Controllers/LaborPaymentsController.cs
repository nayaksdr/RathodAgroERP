using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{

    [ApiController]
    [Route("api/labor-payments")]
    public class LaborPaymentsController : ControllerBase
    {
        private readonly ILaborRepository _laborRepo;
        private readonly ILaborPaymentRepository _laborPaymentRepo;
        private readonly IAttendanceRepository _attendanceRepo;
        private readonly ILaborTypeRateRepository _laborTypeRateRepo;
        private readonly IAdvancePaymentRepository _advanceRepo;
        private readonly ICanePurchaseRepository _canePurchaseRepo;
        private readonly IJaggerySaleRepository _jaggerySaleRepo;
        private readonly IPdfService _pdfService;

        public LaborPaymentsController(
            ILaborRepository laborRepo,
            ILaborPaymentRepository laborPaymentRepo,
            IAttendanceRepository attendanceRepo,
            ILaborTypeRateRepository laborTypeRateRepo,
            IAdvancePaymentRepository advanceRepo,
            ICanePurchaseRepository canePurchaseRepo,
            IJaggerySaleRepository jaggerySaleRepo,
            IPdfService pdfService)
        {
            _laborRepo = laborRepo;
            _laborPaymentRepo = laborPaymentRepo;
            _attendanceRepo = attendanceRepo;
            _laborTypeRateRepo = laborTypeRateRepo;
            _advanceRepo = advanceRepo;
            _canePurchaseRepo = canePurchaseRepo;
            _jaggerySaleRepo = jaggerySaleRepo;
            _pdfService = pdfService;
        }

        // 🔹 GET: api/labor-payments?laborId=&from=&to=
        [HttpGet]
        public async Task<IActionResult> GetPayments(
            int? laborId,
            DateTime? from,
            DateTime? to)
        {
            IEnumerable<Labor> labors = laborId.HasValue
    ? new[] { await _laborRepo.GetByIdAsync(laborId.Value) }
    : await _laborRepo.GetAllLaborsAsync();


            var result = new List<LaborPayment>();

            foreach (var labor in labors.Where(l => l != null))
            {
                var payment = await CalculateLaborPaymentAsync(labor!, from ?? DateTime.MinValue, to ?? DateTime.MaxValue);
                if (payment != null)
                    result.Add(payment);
            }

            return Ok(new
            {
                data = result,
                summary = new
                {
                    totalGross = result.Sum(x => x.GrossAmount),
                    totalAdvance = result.Sum(x => x.AdvanceAdjusted),
                    totalNet = result.Sum(x => x.NetAmount)
                }
            });
        }

        // 🔹 POST: api/labor-payments/generate-slip
        [HttpPost("generate-slip")]
        public async Task<IActionResult> GenerateSlip([FromBody] SlipRequestDto dto)
        {
            var labor = await _laborRepo.GetByIdAsync(dto.LaborId);
            if (labor == null) return NotFound();

            var payment = await CalculateLaborPaymentAsync(labor, dto.FromDate, dto.ToDate);
            if (payment == null) return BadRequest("Calculation failed");

            if (payment.IsPaid)
                return Conflict("Payment already generated");

            await _laborPaymentRepo.AddAsync(payment);
            await _laborPaymentRepo.SaveAsync();

            var pdf = _pdfService.GenerateLaborSlip(
                payment,
                payment.LaborName,
                payment.FromDate,
                payment.ToDate,
                payment.AttendanceDays,
                payment.rate,
                payment.GrossAmount,
                payment.AdvanceAdjusted,
                payment.NetAmount,
                payment.PaymentDate
            );

            return File(pdf, "application/pdf", $"LaborSlip_{labor.Name}.pdf");
        }

        // 🔹 PRIVATE CALCULATION
        private async Task<LaborPayment?> CalculateLaborPaymentAsync(Labor labor, DateTime from, DateTime to)
        {
            decimal gross = 0, rate = 0, work = 0;
            int days = 0;

            var advance = await _advanceRepo.GetAdvanceByLaborInRangeAsync(labor.Id, from, to);
            var laborFull = await _laborRepo.GetByIdWithLaborTypeAsync(labor.Id);

            if (laborFull?.LaborType == null) return null;

            if (laborFull.LaborType.LaborTypeName.Contains("ऊस"))
            {
                work = await _canePurchaseRepo.GetTotalTonsByLaborInRangeAsync(labor.Id, from, to);
                rate = (await _laborTypeRateRepo.GetCurrentRateByLaborTypeIdAsync(labor.LaborTypeId))?.DailyRate ?? 0;
                gross = work * rate;
            }
            else if (laborFull.LaborType.LaborTypeName.Contains("गुळ"))
            {
                work = await _jaggerySaleRepo.GetTotalProductionByLaborInRangeAsync(labor.Id, from, to);
                rate = (await _laborTypeRateRepo.GetCurrentRateByLaborTypeIdAsync(labor.LaborTypeId))?.DailyRate ?? 0;
                gross = work * rate;
            }
            else
            {
                days = await _attendanceRepo.GetDaysPresentAsync(labor.Id, from, to);
                rate = laborFull.LaborType.DailyWage;
                gross = days * rate;
            }

            return new LaborPayment
            {
                LaborId = labor.Id,
                LaborName = labor.Name,
                LaborTypeName = laborFull.LaborType.LaborTypeName,
                WorkAmount = work,
                AttendanceDays = days,
                rate = rate,
                GrossAmount = gross,
                AdvanceAdjusted = advance,
                NetAmount = gross - advance,
                PaymentDate = DateTime.Now,
                FromDate = from,
                ToDate = to
            };
        }
    }
}
