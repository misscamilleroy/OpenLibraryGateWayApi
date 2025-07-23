using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using BookLook.Classes;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BookLook.Services
{
    public interface IGatewayApiService
    {
        public string GetApiUri();
        public Task<BookSearchResults> GetBooksByAuthor(string name, int offset);
    }
    public class GatewayApiService : IGatewayApiService
    {
        private AppConfigOptions _config;

        
        public GatewayApiService(IOptions<AppConfigOptions> options, HttpClient httpClient)
        {
            _config = options.Value;
            httpClient.BaseAddress = new Uri(_config?.OpenLibraryGateWayApiUri ?? "http://unknown");
            httpClient.Timeout = TimeSpan.FromSeconds(60);
            _olGatewayApiClient = httpClient;
        }

        public string GetApiUri()
        {
            return _config.OpenLibraryGateWayApiUri ?? "http://localhost:7024";
        }

        public  HttpClient _olGatewayApiClient { get; }

        public async Task<BookSearchResults> GetBooksByAuthor(string name, int page = 1)
        {
            var queryString = new StringBuilder();
            queryString.Append("search/byname");
            queryString.Append("?authorname=").Append(Uri.EscapeDataString(name));
            queryString.Append("&page=").Append(Convert.ToInt32(page));
            
            using HttpResponseMessage response = await _olGatewayApiClient.GetAsync(queryString.ToString(), HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode)
            {
                //var results =  response.Content.ReadFromJsonAsync<BookSearchResults>(endpointAndSearch);
                var jsonString = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<BookSearchResults>(jsonString);
                return results ?? new BookSearchResults();
            }

            //TODO: add logging
            return new BookSearchResults();

        }
    }
}
