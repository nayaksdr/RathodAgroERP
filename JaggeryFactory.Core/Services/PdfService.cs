using JaggeryAgro.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;

namespace JaggeryAgro.Core.Services
{
    public interface IPdfService
    {
        byte[] GenerateLaborSlip(LaborPayment payment, string laborName, DateTime from, DateTime to);
    }

    public class PdfService : IPdfService
    {
        private readonly IWebHostEnvironment _env;

        public PdfService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public byte[] GenerateLaborSlip(LaborPayment payment, string laborName, DateTime from, DateTime to)
        {
            var logoPath = Path.Combine(_env.WebRootPath, "images", "RJ&A logo.png");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(20);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial Unicode MS"));

                    page.Header()
                        .AlignCenter()
                        .Column(col =>
                        {
                            if (File.Exists(logoPath))
                                col.Item().Image(logoPath, ImageScaling.FitWidth);
                            col.Item().Text("Labor Payment Slip").Bold().FontSize(16);
                        });

                    page.Content().Column(col =>
                    {
                        col.Spacing(10);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(1);
                                c.RelativeColumn(2);
                            });

                            table.Cell().Text("Labor Name:");
                            table.Cell().Text(laborName);

                            table.Cell().Text("Payment Period:");
                            table.Cell().Text($"{from:dd-MM-yyyy} to {to:dd-MM-yyyy}");

                            table.Cell().Text("Payment Date:");
                            table.Cell().Text($"{payment.PaymentDate:dd-MM-yyyy}");
                        });

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn(1);
                                c.RelativeColumn(1);
                            });

                            table.Cell().Text("Gross Salary");
                            table.Cell().Text(payment.GrossAmount.ToString("0.00"));

                            table.Cell().Text("Advance Deducted");
                            table.Cell().Text(payment.AdvanceAdjusted.ToString("0.00"));

                            table.Cell().Text("Net Payable").Bold();
                            table.Cell().Text(payment.NetAmount.ToString("0.00")).Bold();
                        });
                    });

                    page.Footer()
                        .AlignRight()
                        .Text("\nSignature: ______________________");
                });
            });

            return document.GeneratePdf();
        }
    }
}
