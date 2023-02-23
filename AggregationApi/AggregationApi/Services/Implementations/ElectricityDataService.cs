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
        private readonly IFileDownloader _fileDownloader;
        private readonly ICsvReader _csvReader;
        private readonly AppDbContext _context;
        private readonly ILogger<ElectricityDataService> _logger;

        public ElectricityDataService(IFileDownloader contentDownloader, ICsvReader csvReader, AppDbContext context, ILogger<ElectricityDataService> logger)
        {
            _fileDownloader = contentDownloader;
            _csvReader = csvReader;
            _context = context;
            _logger = logger;
        }

        public async Task DownloadDataAndStoreInDatabase()
        {
            var files = await DownloadCsvFiles();

            var data = ReadDataFromCsvFiles(files);

            var interestingData = GetInterestingData(data, Constants.ObtNameFilter);

            await SaveData(interestingData);
        }

        public async Task<List<byte[]>> DownloadCsvFiles()
        {
            _logger.LogInformation("Download started -- {0}", DateTime.Now);
            var csvFiles = new List<byte[]>();
            var fileDownloadTasks = new List<Task>();

            foreach (var url in Constants.CsvFileUrls)
            {
                fileDownloadTasks.Add(Task.Run(async () =>
                {
                    csvFiles.Add(await _fileDownloader.DownloadFileAsync(url));
                }));
            }

            try
            {
                await Task.WhenAll(fileDownloadTasks);
                _logger.LogInformation("Download finished -- {0}", DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while downloading, message = {0}", ex.Message);
            }

            return csvFiles;
        }

        public List<ElectricityDataCsvModel> ReadDataFromCsvFiles(List<byte[]> csvFiles)
        {
            _logger.LogInformation("reading data from files");
            var electricityData = new List<ElectricityDataCsvModel>();

            foreach (var csvFile in csvFiles)
            {
                try
                {
                    var data = _csvReader.Read<ElectricityDataCsvModel>(csvFile);
                    electricityData.AddRange(data);
                }
                catch (Exception ex)
                {
                    _logger.LogError("error while reading csv file. message : {0}", ex.Message);
                }
            }

            return electricityData;
        }

        public List<ElectricityDataCsvModel> GetInterestingData(List<ElectricityDataCsvModel> data, string obtNameFilter)
        {
            _logger.LogInformation("filtering data...");
            return data.Where(x => x.ObtName.ToLower() == obtNameFilter).ToList();
        }

        public async Task SaveData(List<ElectricityDataCsvModel> data)
        {
            _logger.LogInformation("saving data to database");
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
