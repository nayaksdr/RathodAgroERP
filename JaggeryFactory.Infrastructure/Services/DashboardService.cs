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

            return vm;
        }
    }
}
