using ClosedXML.Excel;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Core.Interfaces;
using JaggeryAgro.Core.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace JaggeryAgro.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JaggerySalesReportController : ControllerBase
    {
        private readonly IJaggerySaleRepository _saleRepo;
        private readonly IDealerRepository _dealerRepo;
        private readonly ILogger<JaggerySalesReportController> _logger;

        public JaggerySalesReportController(
            IJaggerySaleRepository saleRepo,
            IDealerRepository dealerRepo,
            ILogger<JaggerySalesReportController> _logger)
        {
            _saleRepo = saleRepo;
            _dealerRepo = dealerRepo;
            _logger = _logger;
        }

        // GET: api/JaggerySalesReport/data
        // Used for the Angular/Frontend Table
        [HttpGet("data")]
        public async Task<IActionResult> GetReportData([FromQuery] int? dealerId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var sales = await _saleRepo.QueryAsync(dealerId, from, to);
            var dealerName = dealerId.HasValue
                ? (await _dealerRepo.GetByIdAsync(dealerId.Value))?.Name
                : "All Dealers";

            var response = new
            {
                DealerName = dealerName,
                From = from,
                To = to,
                Sales = sales ?? new List<JaggerySale>(),
                Summary = new
                {
                    TotalQty = sales?.Sum(s => s.QuantityInKg) ?? 0,
                    TotalAmount = sales?.Sum(s => s.TotalAmount) ?? 0,
                    TotalAdvance = sales?.Sum(s => s.AdvancePaid) ?? 0,
                    TotalRemaining = sales?.Sum(s => s.RemainingAmount) ?? 0
                }
            };

            return Ok(response);
        }

        // GET: api/JaggerySalesReport/export/excel
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel([FromQuery] int? dealerId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var sales = await _saleRepo.QueryAsync(dealerId, from, to);
            var dealerName = dealerId.HasValue ? (await _dealerRepo.GetByIdAsync(dealerId.Value))?.Name : "All Dealers";

            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Sales Report");

            // Styling the Header row
            var headerRow = ws.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#6366f1");
            headerRow.Style.Font.FontColor = XLColor.White;

            string[] headers = { "Dealer", "Date", "Qty (Kg)", "Rate/Kg", "Total", "Advance", "Remaining" };
            for (int i = 0; i < headers.Length; i++) ws.Cell(1, i + 1).Value = headers[i];

            int row = 2;
            foreach (var s in sales)
            {
                ws.Cell(row, 1).Value = s.Dealer?.Name ?? dealerName;
                ws.Cell(row, 2).Value = s.SaleDate.ToString("yyyy-MM-dd");
                ws.Cell(row, 3).Value = s.QuantityInKg;
                ws.Cell(row, 4).Value = s.RatePerKg;
                ws.Cell(row, 5).Value = s.TotalAmount;
                ws.Cell(row, 6).Value = s.AdvancePaid;
                ws.Cell(row, 7).Value = s.RemainingAmount;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            var content = stream.ToArray();

            _logger.LogInformation("Excel Jaggery Sale Report generated via API.");
            var fileName = $"SalesReport_{DateTime.Now:yyyyMMddHHmm}.xlsx";

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // GET: api/JaggerySalesReport/export/pdf
        // NOTE: Rotativa ViewAsPdf works best when pointing to a hidden MVC view 
        // that acts as a template for the PDF.
        [HttpGet("export/pdf")]
        public async Task<IActionResult> ExportPdf([FromQuery] int? dealerId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var sales = await _saleRepo.QueryAsync(dealerId, from, to);
            var dealerName = dealerId.HasValue ? (await _dealerRepo.GetByIdAsync(dealerId.Value))?.Name : "All Dealers";

            var vm = new JaggerySaleReportVM
            {
                DealerId = dealerId,
                DealerName = dealerName,
                From = from,
                To = to,
                Sales = sales
            };

            _logger.LogInformation("PDF Jaggery Sale Report generated via API.");

            return new ViewAsPdf("../Reports/SalesReportPdf", vm)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                FileName = $"SalesReport_{DateTime.Now:yyyyMMddHHmm}.pdf"
            };
        }
        // GET: api/JaggerySalesReport/dealers
        [HttpGet("dealers")]
        public async Task<IActionResult> GetDealers()
        {
            var dealers = await _dealerRepo.GetAllAsync();

            var result = dealers.Select(d => new
            {
                id = d.Id,
                name = d.Name
            });

            return Ok(result);
        }

    }
}