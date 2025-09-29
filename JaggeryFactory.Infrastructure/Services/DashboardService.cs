using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.ViewModel;
using JaggeryAgro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JaggeryAgro.Core.ViewModel.DailyPointDto;

namespace JaggeryAgro.Core.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _ctx;
        
        public DashboardService(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<DashboardDailyVm> GetDailyAsync(int days = 30, CancellationToken ct = default)
        {
            if (days < 1) days = 1;
            var from = DateTime.Today.AddDays(-(days - 1));
            var dates = Enumerable.Range(0, days).Select(i => from.AddDays(i).Date).ToList();
            var labels = dates.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            var vm = new DashboardDailyVm { Labels = labels };

            // ------- Attendance (present count per day)
            // EXPECTED: _ctx.Attendances with fields: Date (DateTime), IsPresent (bool)
            var attRaw = await _ctx.Set<Attendance>()
                .AsNoTracking()
                .Where(a => a.Date >= from)
                .GroupBy(a => a.Date.Date)
                .Select(g => new { Date = g.Key, Count = g.Count(x => x.IsPresent) })
                .ToListAsync(ct);

            vm.AttendanceDaily = dates
                .Select(d => new DailyPointDto(d.ToString("yyyy-MM-dd"), attRaw.FirstOrDefault(x => x.Date == d)?.Count ?? 0))
                .ToList();

            // ------- Advance Payments (₹ per day)
            // EXPECTED: _ctx.AdvancePayments with fields: Date, Amount
            var advRaw = await _ctx.Set<AdvancePayment>()
                .AsNoTracking()
                .Where(x => x.DateGiven >= from)
                .GroupBy(x => x.DateGiven.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(x => (double)x.Amount) })
                .ToListAsync(ct);

            vm.AdvancesDaily = dates
                .Select(d => new DailyPointDto(d.ToString("yyyy-MM-dd"), advRaw.FirstOrDefault(x => x.Date == d)?.Total ?? 0))
                .ToList();

            // ------- Expenses (₹ per day)
            // EXPECTED: _ctx.Expenses with fields: Date, Amount
            var expRaw = await _ctx.Set<Expense>()
                .AsNoTracking()
                .Where(x => x.Date >= from)
                .GroupBy(x => x.Date.Date)
                .Select(g => new { Date = g.Key, Total = g.Sum(x => (double)x.Amount) })
                .ToListAsync(ct);

            vm.ExpensesDaily = dates
                .Select(d => new DailyPointDto(d.ToString("yyyy-MM-dd"), expRaw.FirstOrDefault(x => x.Date == d)?.Total ?? 0))
                .ToList();

            // Cane Purchase (tons)

            var caneRaw = await _ctx.Set<CanePurchase>()
            .AsNoTracking()
            .Where(x => x.PurchaseDate >= from)
            .GroupBy(x => x.PurchaseDate.Date)
            .Select(g => new { Date = g.Key, Tons = g.Sum(x => (double)x.QuantityTon) })
            .ToListAsync(ct);


            vm.CanePurchaseTonsDaily = dates
            .Select(d => new DailyPointDto(d.ToString("yyyy-MM-dd"), caneRaw.FirstOrDefault(x => x.Date == d)?.Tons ?? 0))
            .ToList();

            // Produce
            var prodRaw = await _ctx.Set<JaggeryProduce>()
                .AsNoTracking()
                .Where(x => x.ProducedDate >= from)
                .GroupBy(x => x.ProducedDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Qty = g.Sum(x => (double)x.StockKg)
                })
                .ToListAsync(ct);



            vm.ProduceQtyDaily = dates
            .Select(d => new DailyPointDto(d.ToString("yyyy-MM-dd"), prodRaw.FirstOrDefault(x => x.Date == d)?.Qty ?? 0))
            .ToList();


            //// Sales (Qty + Amount)
            //var saleRaw = await _ctx.Set<JaggerySale>()
            //.AsNoTracking()
            //.Where(x => x.SaleDate >= from)
            //.GroupBy(x => x.SaleDate.Date)
            //.Select(g => new { Date = g.Key, Qty = g.Sum(x => (double)x.QuantityInKg), Amount = g.Sum(x => (double)x.TotalAmount) })
            //.ToListAsync(ct);

            var saleRaw = await _ctx.Set<JaggerySale>()
                .AsNoTracking()
                .Where(x => x.SaleDate >= from)
                .GroupBy(x => x.SaleDate.Date) // Group by the date directly
                .Select(g => new
                {
                    Date = g.Key,
                    Qty = g.Sum(x => (double)x.QuantityInKg),
                    Amount = g.Sum(x => (double)x.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToListAsync(ct);


            // Map to your DailyPointDto (or similar) just like caneRaw
            vm.JagerySellDaily = dates.Select(d =>{
                var dailySales = saleRaw.Where(x => x.Date.Date == d.Date); 
                return new DailyJaggerySellDto(d.ToString("yyyy-MM-dd"),dailySales.Sum(x => x.Qty),dailySales.Sum(x => x.Amount));}).ToList();


            // ------- JaggerySaleShares + Payments (Paid vs Pending per day)
            var shares = await _ctx.Set<JaggerySaleShare>()
                .Include(s => s.JaggerySale)
                .AsNoTracking()
                .Where(s => s.JaggerySale.SaleDate >= from)
                .ToListAsync(ct);

            var payments = await _ctx.Set<JaggerySalePayment>()
                .Include(p => p.JaggerySale)
                .AsNoTracking()
                .Where(p => p.JaggerySale.SaleDate >= from)
                .ToListAsync(ct);

            var shareRaw = shares
                .GroupBy(s => s.JaggerySale.SaleDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalShare = g.Sum(s => s.ShareAmount),
                    PaidFromShare = g.Sum(s => s.PaidAmount)
                })
                .OrderBy(x => x.Date)
                .ToList();

            vm.JaggeryShareDaily = dates
                .Select(d =>
                {
                    var dayShare = shareRaw.FirstOrDefault(x => x.Date == d.Date);
                    var totalShare = dayShare?.TotalShare ?? 0;
                    var paidFromShare = dayShare?.PaidFromShare ?? 0;

                    // Add payments between members for that day
                    var paidFromPayments = payments
                        .Where(p => p.JaggerySale.SaleDate.Date == d.Date)
                        .Sum(p => p.Amount);

                    var totalPaid = paidFromShare + paidFromPayments;
                    var pending = totalShare - totalPaid;

                    return new DailyJaggeryShareDto(
                        d.ToString("yyyy-MM-dd"),
                        totalShare,
                        totalPaid,
                        pending
                    );
                }).ToList();



            // Today tiles
            var today = DateTime.Today;
            vm.TodayPresentCount = await _ctx.Set<Attendance>().CountAsync(a => a.Date.Date == today && a.IsPresent, ct);
            vm.TodayAdvance = await _ctx.Set<AdvancePayment>().Where(x => x.DateGiven.Date == today).SumAsync(x => (double?)x.Amount, ct) ?? 0;
            vm.TodayExpense = await _ctx.Set<Expense>().Where(x => x.Date.Date == today).SumAsync(x => (double?)x.Amount, ct) ?? 0;
            vm.TodayCaneTons = await _ctx.Set<CanePurchase>().Where(x => x.PurchaseDate.Date == today).SumAsync(x => (double?)x.QuantityTon, ct) ?? 0;
            vm.TodayProduceQty = await _ctx.Set<JaggeryProduce>().Where(x => x.ProducedDate.Date == today).SumAsync(x => (double?)x.StockKg, ct) ?? 0;
            //vm.TodayProduceQty = await _ctx.Set<JaggeryProduce>().Where(x => x.ProducedDate.Date == today).SumAsync(x => (double?)x.TotalCost, ct) ?? 0;
            vm.TodaySalesQty = await _ctx.Set<JaggerySale>().Where(x => x.SaleDate.Date == today).SumAsync(x => (double?)x.QuantityInKg, ct) ?? 0;
            vm.TodaySalesAmount = await _ctx.Set<JaggerySale>().Where(x => x.SaleDate.Date == today).SumAsync(x => (double?)x.TotalAmount, ct) ?? 0;
            vm.TodayJaggerySellAmount = await _ctx.Set<JaggerySale>().Where(x => x.SaleDate.Date == today).SumAsync(x => (double?)x.TotalAmount, ct) ?? 0;

            // ------- Today JaggerySaleShares Summary
            var todayShares = await _ctx.Set<JaggerySaleShare>()
                .Include(s => s.JaggerySale)
                .Where(s => s.JaggerySale.SaleDate.Date == today)
                .ToListAsync(ct);

            var todayPayments = await _ctx.Set<JaggerySalePayment>()
                .Include(p => p.JaggerySale)
                .Where(p => p.JaggerySale.SaleDate.Date == today)
                .ToListAsync(ct);

            vm.TodayJaggeryShareTotal = todayShares.Sum(s => s.ShareAmount);
            vm.TodayJaggerySharePaid = todayShares.Sum(s => s.PaidAmount) + todayPayments.Sum(p => p.Amount);
            vm.TodayJaggerySharePending = vm.TodayJaggeryShareTotal - vm.TodayJaggerySharePaid;


            // ------- Weekly Salary Payments (₹ per day)
            // EXPECTED: _ctx.WeeklySalaryPayments (or WeeklySalaries) with fields: PaymentDate, Amount
            //var salRaw = await _ctx.Set<WeeklySalaryPayment>()
            //    .AsNoTracking()
            //    .Where(x => x.PaymentDate >= from)
            //    .GroupBy(x => x.PaymentDate.Date)
            //    .Select(g => new { Date = g.Key, Total = g.Sum(x => (double)x.Amount) })
            //    .ToListAsync(ct);

            //vm.SalaryPaymentsDaily = dates
            //    .Select(d => new DailyPointDto(d.ToString("yyyy-MM-dd"), salRaw.FirstOrDefault(x => x.Date == d)?.Total ?? 0))
            //    .ToList();



            vm.CanePurchaseTonsDaily = dates
                .Select(d => new DailyPointDto(d.ToString("yyyy-MM-dd"), caneRaw.FirstOrDefault(x => x.Date == d)?.Tons ?? 0))
                .ToList();

            // ------- Jaggery Produce (qty per day)
            // EXPECTED: _ctx.Produces with fields: Date, Quantity (double/decimal). Keep unit consistent.
           

            vm.ProduceQtyDaily = dates
                .Select(d => new DailyPointDto(d.ToString("yyyy-MM-dd"), prodRaw.FirstOrDefault(x => x.Date == d)?.Qty ?? 0))
                .ToList();

            vm.JagerySellDaily = dates
                .Select(d => {
                    var dailySales = saleRaw.Where(x => x.Date.Date == d.Date);
                    return new DailyJaggerySellDto(d.ToString("yyyy-MM-dd"),dailySales.Sum(x => x.Qty),dailySales.Sum(x => x.Amount));}).ToList();

            vm.JaggeryShareDaily = dates.Select(d =>{
        // Shares for this day
        var dailyShares = shares.Where(s => s.JaggerySale.SaleDate.Date == d.Date);

        // Total share amount
        var totalShare = dailyShares.Sum(s => s.ShareAmount);

        // Paid from shares
        var paidFromShare = dailyShares.Sum(s => s.PaidAmount);

        // Payments made on this day
        var dailyPayments = payments.Where(p => p.JaggerySale.SaleDate.Date == d.Date);
        var paidFromPayments = dailyPayments.Sum(p => p.Amount);

        // Total paid including payments
        var totalPaid = paidFromShare + paidFromPayments;

        // Pending
        var pending = totalShare - totalPaid;

        return new DailyJaggeryShareDto(
            d.ToString("yyyy-MM-dd"),
            totalShare,
            totalPaid,
            pending
            );
            }).ToList();

            return vm;
        }
    }
}
