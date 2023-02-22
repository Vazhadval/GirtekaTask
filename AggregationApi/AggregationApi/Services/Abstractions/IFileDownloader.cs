namespace AggregationApi.Services.Abstractions
{
    public interface IFileDownloader
    {
        Task<byte[]> DownloadFileAsync(string url);
    }
}
