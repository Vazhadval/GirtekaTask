namespace AggregationApi.Services.Abstractions
{
    public interface IContentDownloader
    {
        Task<byte[]> DownloadContentAsync(string url);
    }
}
