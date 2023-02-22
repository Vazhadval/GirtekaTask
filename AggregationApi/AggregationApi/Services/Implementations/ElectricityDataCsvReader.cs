using AggregationApi.Services.Implementations.Models;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using System.Text;
using System;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.SqlServer.Server;
using AggregationApi.Services.Abstractions;

namespace AggregationApi.Services.Implementations
{
    public class ElectricityDataCsvReader : ICsvReader
    {
        public List<ElectricityDataCsvModel> Read<ElectricityDataCsvModel>(byte[] csvFile)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = ","
            };

            using (var textReader = new StreamReader(new MemoryStream(csvFile), Encoding.UTF8))
            using (var csv = new CsvReader(textReader, configuration))
            {
                csv.Context.RegisterClassMap<CsvDataMap>();
                return csv.GetRecords<ElectricityDataCsvModel>().ToList();
            }
        }
    }
}
