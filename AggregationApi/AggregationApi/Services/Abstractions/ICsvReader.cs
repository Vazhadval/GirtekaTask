using AggregationApi.Services.Implementations.Models;

namespace AggregationApi.Services.Abstractions
{
    public interface ICsvReader
    {
        List<T> Read<T>(byte[] csvFile);
    }
}
