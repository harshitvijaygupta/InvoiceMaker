using InvoiceMaker.Models;

namespace InvoiceMaker.Services.Interface;

public interface IInvoicePdfService
{
    public byte[] GeneratePdf(Invoice invoice);
}
