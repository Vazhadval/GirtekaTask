using AggregationApi.Services.Implementations.Models;

namespace AggregationApi.Services.Abstractions
{
    public interface IElectricityDataCsvReader
    {
        List<ElectricityDataCsvModel> Read(byte[] csvFile);
    }
}
