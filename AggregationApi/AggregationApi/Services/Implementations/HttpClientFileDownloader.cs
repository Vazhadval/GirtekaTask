using AggregationApi.Services.Abstractions;

namespace AggregationApi.Services.Implementations
{
    public class HttpClientFileDownloader : IFileDownloader
    {
        private readonly HttpClient _client;

        public HttpClientFileDownloader(HttpClient client)
        {
            _client = client;
        }

        public async Task<byte[]> DownloadFileAsync(string url)
        {
            return await _client.GetByteArrayAsync(url);
        }
    }
}
