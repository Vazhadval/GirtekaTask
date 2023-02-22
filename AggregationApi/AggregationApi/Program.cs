using AggregationApi.Data;
using AggregationApi.Helpers;
using AggregationApi.Services.Abstractions;
using AggregationApi.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// use "Docker" for docker connection string
string connectionString = builder.Configuration.GetConnectionString("LocalDb");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IFileDownloader, HttpClientFileDownloader>();
builder.Services.AddScoped<ICsvReader, ElectricityDataCsvReader>();
builder.Services.AddScoped<ElectricityDataService>();

builder.Services.AddHttpClient<IFileDownloader, HttpClientFileDownloader>(client =>
{
    client.BaseAddress = new Uri(Constants.ElectricityDataBaseUrl);
    client.Timeout = TimeSpan.FromMinutes(6);
});


Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
                    restrictedToMinimumLevel: LogEventLevel.Information)
               .CreateLogger();

var app = builder.Build();

DbInitializer.ApplyMigrations(app);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
