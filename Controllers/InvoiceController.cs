using InvoiceMaker.Helper;
using InvoiceMaker.Helper.Interface;
using InvoiceMaker.Models;
using InvoiceMaker.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceMaker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoicePdfService _invoicePdfService;
    private readonly IAppLogger _appLogger;
    public InvoiceController(IInvoicePdfService invoicePdfService, IAppLogger appLogger)
    {
        _invoicePdfService = invoicePdfService;
        _appLogger = appLogger;
    }

    [HttpPost("generate-pdf")]
    public IActionResult GeneratePdf([FromBody] Invoice invoice)
    {
        try
        {
            if(invoice == null)
            {
                _appLogger.LogInformation("InvoiceRQ is NULL");
                return BadRequest();
            }
            Random random = new Random();
            invoice.Id = random.Next(1, 99999999);
            _appLogger.LogInformation("Invoice generation started !");
            var pdfBytes = _invoicePdfService.GeneratePdf(invoice);
            if (pdfBytes?.Length > 0)
            {
                _appLogger.LogInformation("Invoice generated !");
                return File(pdfBytes, "application/pdf", $"Invoice_{invoice.Id}.pdf");
            }
            else
                _appLogger.LogWarning("Invoice not generated !");
        }
        catch (Exception ex)
        {
            _appLogger.LogError("Exception occurred while invoice generation ", ex);
        }
        return BadRequest();
    }
}
