using AggregationApi.Data;
using AggregationApi.Data.Entities;
using AggregationApi.Helpers;
using AggregationApi.Services.Abstractions;
using AggregationApi.Services.Implementations.Models;
using Microsoft.EntityFrameworkCore;

namespace AggregationApi.Services.Implementations
{
    public class ElectricityDataService
    {
        private readonly IContentDownloader _contentDownloader;
        private readonly IElectricityDataCsvReader _electricityDataCsvReader;
        private readonly AppDbContext _context;

        public ElectricityDataService(IContentDownloader contentDownloader, IElectricityDataCsvReader electricityDataCsvReader, AppDbContext context)
        {
            _contentDownloader = contentDownloader;
            _electricityDataCsvReader = electricityDataCsvReader;
            _context = context;
        }

        public async Task<List<ElectricityDataCsvModel>> DownloadData()
        {
            var electricityData = new List<ElectricityDataCsvModel>();
            foreach (var url in Constants.CsvFileUrls)
            {
                var csvFile = await _contentDownloader.DownloadContentAsync(url);

                var data = _electricityDataCsvReader.Read(csvFile);
                var apartmentsData = data.Where(x => x.ObtName.ToLower() == Constants.ObtNameFilter);
                electricityData.AddRange(apartmentsData);
            }

            return electricityData;
        }

        public async Task SaveData(List<ElectricityDataCsvModel> data)
        {
            var dataForDb = data.GroupBy(x => x.Network).Select(y => new ElectricityData
            {
                Network = y.Key,
                SumPPlus = y.Sum(t => t.PPlus).Value,
                SumPMinus = y.Sum(t => t.PMinus).Value
            }).ToList();

            _context.ElectricityData.AddRange(dataForDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ElectricityDataViewModel>> GetData()
        {
            return await _context.ElectricityData.Select(x => new ElectricityDataViewModel
            {
                Network = x.Network,
                SumPMinus = x.SumPMinus,
                SumPPlus = x.SumPPlus,
            }).ToListAsync();
        }

        public async Task DeleteData()
        {
            _context.ElectricityData.RemoveRange(_context.ElectricityData);
            await _context.SaveChangesAsync();
        }
    }
}
