using JaggeryAgro.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using System;
using System.IO;

namespace JaggeryAgro.Core.Services
{
    public interface IPdfService
    {
        byte[] GenerateLaborSlip(
              LaborPayment payment,
              string laborName,
              DateTime from,
              DateTime to,
              int attendanceDays,
              decimal rate,
              decimal gross,
              decimal advance,
              decimal net,
              DateTime paymentDate);
    }

    public class PdfService : IPdfService
    {
        private readonly IWebHostEnvironment _env;

        public PdfService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public byte[] GenerateLaborSlip(
             LaborPayment payment,
             string laborName,
             DateTime from,
             DateTime to,
             int attendanceDays,
             decimal rate,
             decimal gross,
             decimal advance,
             decimal net,
             DateTime paymentDate)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var logoPath = Path.Combine(_env.WebRootPath, "images", "RJ&A logo.png");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial Unicode MS"));

                    // ✅ Header
                    page.Header().Column(header =>
                    {
                        if (File.Exists(logoPath))
                        {
                            header.Item().AlignCenter().Height(50).Image(logoPath, ImageScaling.FitHeight);
                        }

                        header.Item().AlignCenter().PaddingTop(5)
                              .Text("👷‍♂️ मजूर वेतन पावती")
                              .FontSize(20)
                              .Bold();

                        header.Item().AlignCenter().Text("राठोड गुळ व कृषी").FontSize(12);
                        header.Item().AlignCenter().PaddingBottom(5)
                              .Text($"वेतन दिनांक: {DateTime.Now:dd-MM-yyyy}");
                    });

                    // ✅ Content
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        // Employee Info Table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(120);
                                columns.RelativeColumn();
                            });

                            table.Cell().Element(CellStyle).Text("मजुराचे नाव:");
                            table.Cell().Element(CellStyle).Text(laborName);

                            table.Cell().Element(CellStyle).Text("मजुराचे नाव:");
                            table.Cell().Element(CellStyle).Text($"{from:dd-MM-yyyy} ते {to:dd-MM-yyyy}");

                            table.Cell().Element(CellStyle).Text("वेतन दिनांक:");
                            table.Cell().Element(CellStyle).Text($"{paymentDate:dd-MM-yyyy}");
                        });

                        col.Item().PaddingVertical(10);

                        // Salary Details Table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).Text("वर्णन");
                                header.Cell().Element(HeaderCellStyle).AlignRight().Text("रक्कम (₹)");
                            });

                            AddRow(table, "हजेरीचे दिवस", attendanceDays);
                            AddRow(table, "मजुरीचा दर", rate);
                            AddRow(table, "एकूण वेतन", gross);
                            AddRow(table, "आगाऊ रक्कम वजा", advance);
                            AddRow(table, "देय रक्कम", net, bold: true);
                        });

                        // ✅ Footer
                        col.Item().PaddingTop(20).AlignRight().Text("अधिकृत स्वाक्षरी: ____________________");
                    });

                    // ✅ Helper Methods for Styling
                    static IContainer CellStyle(IContainer container)
                    {
                        return container.Padding(3).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2);
                    }

                    static IContainer HeaderCellStyle(IContainer container)
                    {
                        return container.Background(Colors.Grey.Lighten3)
                                        .Padding(5)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.Grey.Darken2);
                    }

                    static void AddRow(TableDescriptor table, string label, decimal amount, bool bold = false)
                    {
                        if (bold)
                        {
                            table.Cell().Element(CellStyle).Text(label).FontSize(12).Bold();
                            table.Cell().Element(CellStyle).AlignRight().Text($"{amount:F2}").Bold();
                        }
                        else
                        {
                            table.Cell().Element(CellStyle).Text(label).FontSize(12);
                            table.Cell().Element(CellStyle).AlignRight().Text($"{amount:F2}");
                        }
                    }

                });
            });

            return document.GeneratePdf();
        }
    }
}
