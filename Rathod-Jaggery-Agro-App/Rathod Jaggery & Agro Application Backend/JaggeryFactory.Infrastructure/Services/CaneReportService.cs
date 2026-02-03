using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JaggeryAgro.Infrastructure.Services
{
    public class CaneReportService
    {
        private readonly ICanePurchaseRepository _purchaseRepo;
        private readonly ICanePaymentRepository _paymentRepo;

        public CaneReportService(ICanePurchaseRepository purchaseRepo, ICanePaymentRepository paymentRepo)
        {
            _purchaseRepo = purchaseRepo;
            _paymentRepo = paymentRepo;
        }
        public async Task<FarmersCaneReportDto> GetFarmerReportAsync(int farmerId)
        {
            var purchases = await _purchaseRepo.GetByFarmerAsyncNew(farmerId);
            var payments = await _paymentRepo.GetByFarmerAsyncNew(farmerId);

            var totalTons = purchases.Sum(p => p.QuantityTon);
            var totalPayable = purchases.Sum(p => p.TotalAmountSnapshot);

            var totals = payments
                .GroupBy(p => p.PaymentType)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.NetAmount));

            var totalPaid = totals.GetValueOrDefault(PaymentType.Final, 0);
            var totalAdvance = totals.GetValueOrDefault(PaymentType.Advance, 0);


            return new FarmersCaneReportDto
            {
                FarmerId = farmerId,
                FarmerName = purchases.FirstOrDefault()?.Farmer?.Name ?? "",
                TotalTons = totalTons,
                TotalPayable = totalPayable,
                TotalPaid = totalPaid,
                TotalAdvance = totalAdvance,
                Balance = totalPayable - (totalPaid + totalAdvance)
            };
        }
    }
}
