using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OpenLibraryGateWayApi.Tests.MockHttpClientClasses;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly string _response;
    private readonly HttpStatusCode _statusCode;

    public string Input { get; private set; }
    public int NumberOfCalls { get; private set; }
    public HttpStatusCode StatusCode { get; private set; }

    public MockHttpMessageHandler(string response, HttpStatusCode statusCode)
    {
        _response = response;
        _statusCode = statusCode;
        Input = "";
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        NumberOfCalls++;

        if (request.Content != null)
        {
            Input = await request.Content.ReadAsStringAsync();
        }
        StatusCode = _statusCode;

        return new HttpResponseMessage
        {
            StatusCode = _statusCode,
            Content = new StringContent(_response)
        };
    }
}
