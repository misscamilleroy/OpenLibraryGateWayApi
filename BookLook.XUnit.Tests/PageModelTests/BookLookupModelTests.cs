using System.Net;
using BookLook.Classes;
using BookLook.Pages.Book;
using BookLook.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NSubstitute;
using OpenLibraryGateWayApi.Tests.MockHttpClientClasses;

namespace BookLook.XUnit.Tests;

public class BookLookupModelTests
{
    ILogger<BookLookupModel> _logger;
    IOptions<AppConfigOptions> _options;
    EmptyModelMetadataProvider _modelMetadataProvider = new EmptyModelMetadataProvider();
    ViewDataDictionary _viewData;
    PageContext _pageContext;

    public BookLookupModelTests()
    {
        _logger = Substitute.For<ILogger<BookLookupModel>>();
        _options = Substitute.For<IOptions<AppConfigOptions>>();
        _viewData = new ViewDataDictionary(_modelMetadataProvider, new ModelStateDictionary());
        _pageContext = new PageContext { ViewData = _viewData };
    }

    [Fact]
    public async Task HappyPath_BookLookPage_Returns_ExpectedData()
    {
        // Arrange
        var namesAndDates = new[]{
                   new {Name = "A", Year = 2000},
                   new {Name = "Z", Year = 2000}
               };
        var jsonTestResult = CreateJsonResultString(namesAndDates);
        var mockMessageHandler = new MockGatewayHttpMessageHandler(jsonTestResult, HttpStatusCode.OK);
        var httpClient = new HttpClient(mockMessageHandler);
        var gatewayService = new GatewayApiService(_options, httpClient);

        var bookLookupPageModel = new BookLookupModel(_logger, gatewayService)
        {
            PageContext = _pageContext,
            MetadataProvider = _modelMetadataProvider // Also set the MetadataProvider
        };        

        // Act
        await bookLookupPageModel.OnGetAsync("any", 1);

        // Assert
        mockMessageHandler.NumberOfCalls.Should().Be(1);
        mockMessageHandler.StatusCode.Should().Be(HttpStatusCode.OK);
        bookLookupPageModel.ViewData.Should().NotBeNull();
        _viewData["currentSearch"].Should().Be("any");
        _viewData["requestedPage"].Should().Be(1);
        _viewData["SearchResults"].Should().NotBeNull();
        _viewData["Docs"].Should().NotBeNull();
    }

    [Fact]
    public async Task BookLookPage_Returns_Expected_EmptyData_WhenGateway_Returns_503()
    {
        // Arrange
        var mockMessageHandler = new MockGatewayHttpMessageHandler("", HttpStatusCode.ServiceUnavailable);
        var httpClient = new HttpClient(mockMessageHandler);
        var gatewayService = new GatewayApiService(_options, httpClient);

        var bookLookupPageModel = new BookLookupModel(_logger, gatewayService)
        {
            PageContext = _pageContext,
            MetadataProvider = _modelMetadataProvider // Also set the MetadataProvider
        };

        // Act
        await bookLookupPageModel.OnGetAsync("any", 1);

        // Assert
        mockMessageHandler.NumberOfCalls.Should().Be(1);
        mockMessageHandler.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);

        bookLookupPageModel.ViewData.Should().NotBeNull();
        _viewData["currentSearch"].Should().Be("any");
        _viewData["requestedPage"].Should().Be(1);
        _viewData["SearchResults"].Should().NotBeNull();
        _viewData["Docs"].Should().NotBeNull();
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
