using ClosedXML.Excel;
using JaggeryAgro.Core.DTOs;
using JaggeryAgro.Core.Entities;
using JaggeryAgro.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JaggeryAgro.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduceController : ControllerBase
    {
        private readonly IProduceService _service;
        private readonly ILogger<ProduceController> _logger;

        public ProduceController(IProduceService service, ILogger<ProduceController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ================= LIST =================
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var list = await _service.ListAsync(from, to);

            return Ok(new
            {
                data = list,
                totalUnitPrice = list.Sum(x => x.UnitPrice),
                totalCost = list.Sum(x => x.TotalCostSnapshot)
            });
        }

        // ================= DETAILS =================
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.GetAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        // ================= CREATE =================
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProduceCreateDto dto)
        {
            var model = new JaggeryProduce
            {
                ProducedDate = dto.ProducedDate,
                BatchNumber = string.IsNullOrWhiteSpace(dto.BatchNumber)
                    ? $"B-{DateTime.Now:yyyyMMddHHmmss}"
                    : dto.BatchNumber,
                QuantityKg = dto.QuantityKg,
                UnitPrice = dto.UnitPrice,
                QualityGrade = dto.QualityGrade,
                Notes = dto.Notes
            };

            var (success, error) = await _service.CreateAsync(model);
            if (!success) return BadRequest(error);

            _logger.LogInformation("Created Jaggery Produce");
            return Ok(model);
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProduceUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("Invalid ID");

            var model = await _service.GetAsync(id);
            if (model == null) return NotFound();

            model.ProducedDate = dto.ProducedDate;
            model.QuantityKg = dto.QuantityKg;
            model.UnitPrice = dto.UnitPrice;
            model.QualityGrade = dto.QualityGrade;
            model.Notes = dto.Notes;

            var (success, error) = await _service.UpdateAsync(model);
            if (!success) return BadRequest(error);

            _logger.LogInformation("Updated Jaggery Produce");
            return Ok(model);
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            _logger.LogInformation("Deleted Jaggery Produce");
            return NoContent();
        }

        // ================= EXPORT EXCEL =================
        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportExcel(DateTime? from, DateTime? to)
        {
            var list = await _service.ListAsync(from, to);

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Jaggery Produce");

            ws.Cell(1, 1).Value = "Produced Date";
            ws.Cell(1, 2).Value = "Batch";
            ws.Cell(1, 3).Value = "Qty (Kg)";
            ws.Cell(1, 4).Value = "Unit Price";
            ws.Cell(1, 5).Value = "Total Cost";
            ws.Cell(1, 6).Value = "Grade";
            ws.Cell(1, 7).Value = "Stock";
            ws.Cell(1, 8).Value = "Notes";

            int row = 2;
            foreach (var p in list)
            {
                ws.Cell(row, 1).Value = p.ProducedDate;
                ws.Cell(row, 2).Value = p.BatchNumber;
                ws.Cell(row, 3).Value = p.QuantityKg;
                ws.Cell(row, 4).Value = p.UnitPrice;
                ws.Cell(row, 5).Value = p.TotalCostSnapshot;
                ws.Cell(row, 6).Value = p.QualityGrade;
                ws.Cell(row, 7).Value = p.StockKg;
                ws.Cell(row, 8).Value = p.Notes;
                row++;
            }

            ws.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"JaggeryProduce_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }
    }
}

