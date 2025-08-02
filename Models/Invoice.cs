namespace InvoiceMaker.Models;

public class Invoice
{
    public int Id { get; set; }
    public Client Client { get; set; } = new();
    public List<InvoiceItem> Items { get; set; } = new();
    public DateTime InvoiceDate { get; set; } = DateTime.Now;
    public decimal TaxRate { get; set; } = 0.18m;
    public decimal SubTotal => Items?.Sum(i => i.Quantity * i.UnitPrice) ?? 0;
    public decimal Tax => SubTotal * TaxRate;
    public decimal Total => SubTotal + Tax;
}
