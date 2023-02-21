using AggregationApi.Services.Abstractions;

namespace AggregationApi.Services.Implementations
{
    public class HttpClientContentDownloader : IContentDownloader
    {
        private readonly HttpClient _client;

        public HttpClientContentDownloader(HttpClient client)
        {
            _client = client;
        }

        public async Task<byte[]> DownloadContentAsync(string url)
        {
            return await _client.GetByteArrayAsync(url);
        }
    }
}
