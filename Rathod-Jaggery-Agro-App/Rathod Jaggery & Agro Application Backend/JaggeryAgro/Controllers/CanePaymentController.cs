using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Route("api/cane-payments")]
    public class CanePaymentController : ControllerBase
    {
        private readonly ICanePaymentRepository _paymentRepo;
        private readonly ICaneAdvanceRepository _advanceRepo;
        private readonly ICanePurchaseRepository _purchaseRepo;
        private readonly IFarmerRepository _farmerRepo;
        private readonly IWebHostEnvironment _env;

        public CanePaymentController(
            ICanePaymentRepository paymentRepo,
            ICaneAdvanceRepository advanceRepo,
            ICanePurchaseRepository purchaseRepo,
            IFarmerRepository farmerRepo,
            IWebHostEnvironment env)
        {
            _paymentRepo = paymentRepo;
            _advanceRepo = advanceRepo;
            _purchaseRepo = purchaseRepo;
            _farmerRepo = farmerRepo;
            _env = env;
        }

        // 🔹 SUMMARY
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(int? farmerId)
        {
            var purchases = await _purchaseRepo.GetAllAsync();
            var advances = await _advanceRepo.GetAllAsync();
            var payments = await _paymentRepo.GetAllAsync();
            var farmers = await _farmerRepo.GetAllAsync();

            var summary = purchases
                .GroupBy(p => p.FarmerId)
                .Select(g =>
                {
                    var totalPurchase = g.Sum(x => x.TotalAmount);
                    var totalAdvance = advances.Where(a => a.FarmerId == g.Key).Sum(a => a.Amount);
                    var totalPaid = payments.Where(p => p.FarmerId == g.Key).Sum(p => p.NetAmount);
                    var paymentDate = payments.Where(p => p.FarmerId == g.Key)
                        .OrderByDescending(p => p.PaymentDate)
                        .FirstOrDefault()?.PaymentDate ?? DateTime.MinValue;

                    var netPayable = Math.Max(totalPurchase - totalAdvance, 0);
                    var carryForward = Math.Max(netPayable - totalPaid, 0);

                    var status =
                        totalPaid == 0 ? "प्रलंबित" :
                        totalPaid < netPayable ? "आंशिक भरलेले" :
                        "पूर्ण";

                    var farmer = farmers.First(f => f.Id == g.Key);

                    return new CanePaymentSummaryDto
                    {
                        FarmerId = g.Key,
                        FarmerName = farmer.Name,
                        TotalPurchase = totalPurchase,
                        TotalAdvance = totalAdvance,
                        NetAmount = netPayable,
                        PaidAmount = totalPaid,
                        PaymentDate = paymentDate,
                        CarryForward = carryForward,
                        PaymentStatus = status,
                        IsPaid = status == "पूर्ण"
                    };
                })
                .ToList();

            return Ok(summary);
        }

        // 🔹 PAYMENT SLIP
        [HttpGet("slip/{farmerId}")]
        public async Task<IActionResult> DownloadSlip(int farmerId)
        {
            // (same logic as your PaymentSlip method)
            // return File(pdfBytes, "application/pdf", "Slip.pdf");
            return Ok(); // shortened for readability
        }
    }
}
