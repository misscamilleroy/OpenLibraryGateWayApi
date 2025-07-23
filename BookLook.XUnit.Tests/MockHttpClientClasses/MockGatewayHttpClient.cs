using Newtonsoft.Json;
using BookLook.Classes;

namespace BookLook.XUnit.Tests;

public class MockGatewayHttpClient 
{

    private readonly HttpClient _httpClient;

    public MockGatewayHttpClient(HttpMessageHandler handler)
    {
        _httpClient = new HttpClient(handler); 
    }

    public async Task<BookSearchResults> GetOpenLibrarySearchResults(string search, int page = 1)
    {
        var response = await _httpClient.GetAsync("mockclientdoesntcare");
        var responseBody = await response.Content.ReadAsStringAsync();

        var results = JsonConvert.DeserializeObject<BookSearchResults>(responseBody);

        return results ?? new BookSearchResults();
    }

}
