using AggregationApi.Data;
using AggregationApi.Helpers;
using AggregationApi.Services.Abstractions;
using AggregationApi.Services.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IContentDownloader, HttpClientContentDownloader>();
builder.Services.AddScoped<IElectricityDataCsvReader, ElectricityDataCsvReader>();
builder.Services.AddScoped<ElectricityDataService>();

builder.Services.AddHttpClient<IContentDownloader, HttpClientContentDownloader>(client =>
{
    client.BaseAddress = new Uri(Constants.ElectricityDataBaseUrl);
    client.Timeout = TimeSpan.FromMinutes(6);
});

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
