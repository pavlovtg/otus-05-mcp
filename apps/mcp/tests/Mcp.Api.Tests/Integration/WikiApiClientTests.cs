using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Mcp.Api.Clients;
using Mcp.Api.Models;

namespace Mcp.Api.Tests.Integration;

public class WikiApiClientTests
{
	private static HttpClient CreateClient(HttpMessageHandler handler)
	{
		var client = new HttpClient(handler)
		{
			BaseAddress = new Uri("http://localhost:5000"),
		};
		return client;
	}

	[Fact]
	public async Task GetContractsAsync_ReturnsContracts()
	{
		var contracts = new List<ContractInfo>
		{
			new() { Name = "Service1.yaml", Title = "Service 1", Version = "1.0", Description = "Desc" },
		};
		var json = JsonSerializer.Serialize(contracts);
		var handler = new MockHttpMessageHandler(HttpStatusCode.OK, json, "application/json");
		var wikiClient = new WikiApiClient(CreateClient(handler));

		var result = await wikiClient.GetContractsAsync();

		Assert.Single(result);
		Assert.Equal("Service1.yaml", result[0].Name);
	}

	[Fact]
	public async Task SearchContractsAsync_ReturnsFilteredContracts()
	{
		var contracts = new List<ContractInfo>
		{
			new() { Name = "Analytics.yaml", Title = "Analytics API", Version = "1.0", Description = "Analytics" },
		};
		var json = JsonSerializer.Serialize(contracts);
		var handler = new MockHttpMessageHandler(HttpStatusCode.OK, json, "application/json");
		var wikiClient = new WikiApiClient(CreateClient(handler));

		var result = await wikiClient.SearchContractsAsync("analytics");

		Assert.Single(result);
		Assert.Equal("Analytics.yaml", result[0].Name);
	}

	[Fact]
	public async Task GetContractAsync_ReturnsYamlContent()
	{
		var yaml = "openapi: 3.0.0\ninfo:\n  title: Test";
		var handler = new MockHttpMessageHandler(HttpStatusCode.OK, yaml, "application/yaml");
		var wikiClient = new WikiApiClient(CreateClient(handler));

		var result = await wikiClient.GetContractAsync("Service1.yaml");

		Assert.Equal(yaml, result);
	}

	[Fact]
	public async Task GetContractsAsync_WhenUnavailable_ThrowsWikiApiException()
	{
		var handler = new MockHttpMessageHandler(new HttpRequestException("Connection refused"));
		var wikiClient = new WikiApiClient(CreateClient(handler));

		await Assert.ThrowsAsync<WikiApiException>(() => wikiClient.GetContractsAsync());
	}

	[Fact]
	public async Task GetContractAsync_WhenNotFound_ThrowsWikiApiException()
	{
		var handler = new MockHttpMessageHandler(HttpStatusCode.NotFound, "", "application/json");
		var wikiClient = new WikiApiClient(CreateClient(handler));

		await Assert.ThrowsAsync<WikiApiException>(() => wikiClient.GetContractAsync("nonexistent.yaml"));
	}
}

internal class MockHttpMessageHandler : HttpMessageHandler
{
	private readonly HttpStatusCode _statusCode;
	private readonly string _content;
	private readonly string _contentType;
	private readonly HttpRequestException? _exception;

	public MockHttpMessageHandler(HttpStatusCode statusCode, string content, string contentType)
	{
		_statusCode = statusCode;
		_content = content;
		_contentType = contentType;
	}

	public MockHttpMessageHandler(HttpRequestException exception)
	{
		_exception = exception;
		_statusCode = HttpStatusCode.OK;
		_content = string.Empty;
		_contentType = string.Empty;
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (_exception != null)
			throw _exception;

		var response = new HttpResponseMessage(_statusCode)
		{
			Content = new StringContent(_content, System.Text.Encoding.UTF8, _contentType),
		};
		return Task.FromResult(response);
	}
}
