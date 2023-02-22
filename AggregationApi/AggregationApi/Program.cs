using AggregationApi.Data;
using AggregationApi.Helpers;
using AggregationApi.Services.Abstractions;
using AggregationApi.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));

builder.Services.AddScoped<IContentDownloader, HttpClientContentDownloader>();
builder.Services.AddScoped<IElectricityDataCsvReader, ElectricityDataCsvReader>();
builder.Services.AddScoped<ElectricityDataService>();

builder.Services.AddHttpClient<IContentDownloader, HttpClientContentDownloader>(client =>
{
    client.BaseAddress = new Uri(Constants.ElectricityDataBaseUrl);
    client.Timeout = TimeSpan.FromMinutes(6);
});


Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.MSSqlServer(
                    builder.Configuration.GetSection("Serilog:ConnectionStrings:LogDatabase").Value,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Log", AutoCreateSqlTable = true }
               , null, null, LogEventLevel.Information, null, columnOptions: null, null, null)
               .CreateLogger();

var app = builder.Build();

DbInitializer.Initialize(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
