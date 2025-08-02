using InvoiceMaker.Helper.Interface;
using InvoiceMaker.Helper;
using InvoiceMaker.Services;
using InvoiceMaker.Services.Interface;
using QuestPDF.Infrastructure;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-IN");
// Add services to the container.
builder.Services.AddSingleton<IInvoicePdfService, InvoicePdfService>();
builder.Services.AddSingleton<IAppLogger, AppLogger>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
