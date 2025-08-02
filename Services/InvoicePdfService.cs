using QuestPDF.Fluent;
using QuestPDF.Helpers;
using InvoiceMaker.Models;
using InvoiceMaker.Services.Interface;
using InvoiceMaker.Helper.Interface;
using QuestPDF.Infrastructure;

namespace InvoiceMaker.Services;

public class InvoicePdfService : IInvoicePdfService
{
    private readonly IAppLogger _appLogger;
    public InvoicePdfService(IAppLogger appLogger)
    {
        _appLogger = appLogger;
    }

    public byte[] GeneratePdf(Invoice invoice)
    {
        try
        {
            var logoPath = Path.Combine("C:\\Users\\harsh\\source\\repos\\InvoiceMaker\\logo.png");
            byte[] logoImage = File.Exists(logoPath) ? File.ReadAllBytes(logoPath) : Array.Empty<byte>();
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(col =>
                    {
                        // Header
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(col =>
                            {
                                col.Item().Text("The Gupta's").FontSize(20).Bold();
                                col.Item().Text("theguptas@gmail.com");
                                col.Item().Text("https://project-title-harshit.my.canva.site/");
                            });
                            if (logoImage?.Length > 0)
                                row.ConstantItem(80).Height(50).Image(logoImage);
                            else
                                row.ConstantItem(80).Height(50).Placeholder();
                        });

                        col.Item().PaddingVertical(10).LineHorizontal(1);

                        // Invoice Info
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Text($"Invoice #: {invoice.Id}").Bold();
                            row.RelativeItem().AlignRight().Text($"Date: {invoice.InvoiceDate:MM/dd/yyyy}").Bold();
                        });

                        col.Item().Text("Bill To:").Bold().FontSize(14);
                        col.Item().Text(invoice.Client.Name);
                        col.Item().Text(invoice.Client.Address);
                        col.Item().Text(invoice.Client.Email);

                        col.Item().PaddingVertical(10).LineHorizontal(1);

                        // Table
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Description
                                columns.RelativeColumn();  // Qty
                                columns.RelativeColumn();  // Price
                                columns.RelativeColumn();  // Total
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Description").Bold();
                                header.Cell().Element(CellStyle).AlignRight().Text("Qty").Bold();
                                header.Cell().Element(CellStyle).AlignRight().Text("Price").Bold();
                                header.Cell().Element(CellStyle).AlignRight().Text("Total").Bold();
                            });

                            foreach (var item in invoice.Items ?? new())
                            {
                                table.Cell().Element(DataCell).Text(item.Description ?? "");
                                table.Cell().Element(DataCell).AlignRight().Text(item.Quantity.ToString());
                                table.Cell().Element(DataCell).AlignRight().Text(item.UnitPrice.ToString("C"));
                                table.Cell().Element(DataCell).AlignRight().Text((item.Quantity * item.UnitPrice).ToString("C"));
                            }

                            static IContainer CellStyle(IContainer container) =>
                                container.Padding(5).Background("#F0F0F0").BorderBottom(1);

                            static IContainer DataCell(IContainer container) =>
                                container.BorderBottom(1).PaddingVertical(3);
                        });

                        // Totals
                        col.Item().AlignRight().Column(totals =>
                        {
                            totals.Item().Text($"Subtotal: {invoice.SubTotal:C}");
                            totals.Item().Text($"Tax ({invoice.TaxRate:P0}): {invoice.Tax:C}");
                            totals.Item().LineHorizontal(1);
                            totals.Item().Text($"Total: {invoice.Total:C}").FontSize(14).Bold();
                        });

                        // Footer
                        col.Item().PaddingTop(25).Text("Thank you for your business!").Italic().AlignCenter();
                    });
                });
            });

            return document.GeneratePdf();
        }
        catch(Exception ex)
        {
            _appLogger.LogError("Exception in GeneratePdf", ex);
        }
        return null;
    }
}
