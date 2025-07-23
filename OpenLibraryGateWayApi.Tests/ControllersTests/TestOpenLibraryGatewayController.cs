using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using OpenLibraryGateWayApi.ApiModels;
using OpenLibraryGateWayApi.Controllers;
using OpenLibraryGateWayApi.Services;
using OpenLibraryGateWayApi.Tests.MockHttpClientClasses;
using Xunit;

namespace OpenLibraryGateWayApi.Tests.ControllerTests;

public class TestOpenLibraryGatewayController
{
    // private readonly IContactOpenLibraryService _openLibraryService;
    private ILogger<OpenLibraryGatewayController> _logger;
    public TestOpenLibraryGatewayController()
    {
        _logger = Substitute.For<ILogger<OpenLibraryGatewayController>>();
    }

    [Fact]
    public async Task HappyPath_GetSearchResults_UseFluentAssertions()
    {
        // Arrange
        var namesAndDates = new[]{
                   new {Name = "A", Year = 2000},
                   new {Name = "Z", Year = 2000}
               };
        var jsonTestResult = CreateJsonResultString(namesAndDates);
        var expectedSearchResult = JsonConvert.DeserializeObject<OpenLibrarySearchResult>(jsonTestResult) as OpenLibrarySearchResult;

        var messageHandler = new MockHttpMessageHandler(jsonTestResult, HttpStatusCode.OK);
        var httpClient = new HttpClient(messageHandler);
        var mockedOlClient = new ContactOpenLibraryService(httpClient);
        var sut = new OpenLibraryGatewayController(_logger, mockedOlClient);

        // Act
        var actionResult = await sut.GetSearchResults("any", 1);
        var resultObject = actionResult.Result as ObjectResult;
        var searchResultObject = resultObject?.Value as OpenLibrarySearchResult;

        //Assert
        messageHandler.NumberOfCalls.Should().Be(1);
        searchResultObject?.docs?.Length.Should().Be(namesAndDates.Count());
        resultObject?.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetSearchResults_Returns503_When_OpenLibrary_Returns503_UseXUnitAssertions()
    {
        // Arrange
        var messageHandler = new MockHttpMessageHandler("", HttpStatusCode.ServiceUnavailable);
        var httpClient = new HttpClient(messageHandler);
        var mockedOlClient = new ContactOpenLibraryService(httpClient);
        var sut = new OpenLibraryGatewayController(_logger, mockedOlClient);

        // Act
        var actionResult = await sut.GetSearchResults("any", 1);
        var resultObject = actionResult.Result as ObjectResult;

        //Assert
        Assert.Equal(1, messageHandler.NumberOfCalls);
        Assert.NotNull(resultObject);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, resultObject.StatusCode);
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
                edition_count = 0,
                key = "",
                language = new[] { "" },
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
