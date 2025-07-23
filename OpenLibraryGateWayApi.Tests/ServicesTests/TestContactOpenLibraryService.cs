using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using OpenLibraryGateWayApi.Controllers;
using OpenLibraryGateWayApi.Services;
using OpenLibraryGateWayApi.Tests.MockHttpClientClasses;
using Xunit;


namespace OpenLibraryGateWayApi.Tests.ServiceTests;
public class TestContactOpenLibraryService
{
   // private readonly IContactOpenLibraryService _openLibraryService;
    private ILogger<OpenLibraryGatewayController> _logger;
    public TestContactOpenLibraryService()
    {
        _logger = Substitute.For<ILogger<OpenLibraryGatewayController>>();       
    }   

    [Fact]
    public async Task GetSearchResults_Sorts_By_Author_And_Date()
    {
        // Arrange
        var namesAndDates =  new[]{
                   new {Name = "A", Year = 2000},
                   new {Name = "Z", Year = 2000},
                   new {Name = "A", Year = 1950},
                   new {Name = "Z", Year = 1950},
               };
        var unorderedSampleOpenLibraryJson = CreateJsonResultString(namesAndDates);

        var mockMessageHandler = new MockHttpMessageHandler(unorderedSampleOpenLibraryJson, HttpStatusCode.OK);
        var mockHttpClient = new HttpClient(mockMessageHandler);
        var contactOlService = new ContactOpenLibraryService(mockHttpClient);

        // Act
        var searchResult = await contactOlService.GetOpenLibrarySearchResults("any", 1);

        //Assert
        Assert.Equal("A", searchResult?.docs?[0]?.author_name?.First());
        Assert.Equal(1950, searchResult?.docs?[0]?.first_publish_year);

        Assert.Equal("A", searchResult?.docs?[1]?.author_name?.First());
        Assert.Equal(2000, searchResult?.docs?[1]?.first_publish_year);

        Assert.Equal("Z", searchResult?.docs?[2]?.author_name?.First());
        Assert.Equal(1950, searchResult?.docs?[2]?.first_publish_year);

        Assert.Equal("Z", searchResult?.docs?[3]?.author_name?.First());
        Assert.Equal(2000, searchResult?.docs?[3]?.first_publish_year);
    }

    [Fact]
    public async Task GetSearchResults_Throws_When_OpenLibrary_Returns503()
    {
        // Arrange

        var messageHandler = new MockHttpMessageHandler("", HttpStatusCode.ServiceUnavailable);
        var httpClient = new HttpClient(messageHandler);
        var contactOlService = new ContactOpenLibraryService(httpClient);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => contactOlService.GetOpenLibrarySearchResults("any", 1));
    }

    private string CreateJsonResultString(dynamic namesAndDates)
    {
        var jsonDocs = new List<dynamic>();
        foreach (var element in namesAndDates)
        {
            jsonDocs.Add(new
            {
                author_name = new[] { element.Name },
                first_publish_year = element.Year,
                author_key = new[] { "" },
                cover_edition_key = "",
                ebook_access = "",
                edition_count = 25,
                key = "",
                language = new[] { ""},
                title = "",
                subject = new[] { "" }
            });
        }

        var jsonObj = new
        {
            numFound = 0,
            start = 0,
            numFoundExact = true,
            num_found = 0,
            documentation_url = "",
            q = "",
            offset = 0,
            docs = jsonDocs
        };
        
        return JsonConvert.SerializeObject(jsonObj);
    }
}
