using System.Net;
using System.Text;
using System.Text.Json;
using Mcp.Api.Clients;
using Mcp.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Mcp.Api.Tests.Api;

public class McpApiTests : IClassFixture<WebApplicationFactory<Program>>
{
	private readonly WebApplicationFactory<Program> _factory;

	public McpApiTests(WebApplicationFactory<Program> factory)
	{
		_factory = factory;
	}

	private WebApplicationFactory<Program> CreateFactory(IReadOnlyList<ContractInfo>? contracts = null, string? contractYaml = null)
	{
		contracts ??= [
			new() { Name = "Service1.yaml", Title = "Service 1", Version = "1.0", Description = "Desc 1" },
			new() { Name = "Analytics.yaml", Title = "Analytics API", Version = "2.0", Description = "Analytics" },
		];
		contractYaml ??= "openapi: 3.0.0\ninfo:\n  title: Service 1";

		var contractsJson = JsonSerializer.Serialize(contracts);
		var yaml = contractYaml;

		return _factory.WithWebHostBuilder(builder =>
		{
			builder.ConfigureServices(services =>
			{
				var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IWikiApiClient));
				if (descriptor != null) services.Remove(descriptor);

				services.AddHttpClient<IWikiApiClient, WikiApiClient>(client =>
				{
					client.BaseAddress = new Uri("http://mock-wiki-api");
				}).ConfigurePrimaryHttpMessageHandler(() =>
					new MockWikiApiHandler(contractsJson, yaml));
			});
		});
	}

	private static StringContent McpJson(object payload) =>
		new(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

	private static HttpRequestMessage McpRequest(object payload) =>
		new(HttpMethod.Post, "/")
		{
			Content = McpJson(payload),
			Headers = { { "Accept", "application/json, text/event-stream" } },
		};

	[Fact]
	public async Task McpEndpoint_Initialize_ReturnsSuccess()
	{
		var client = CreateFactory().CreateClient();

		var response = await client.SendAsync(McpRequest(new
		{
			jsonrpc = "2.0",
			id = 1,
			method = "initialize",
			@params = new
			{
				protocolVersion = "2024-11-05",
				capabilities = new { },
				clientInfo = new { name = "test", version = "1.0" },
			},
		}));
		var body = await response.Content.ReadAsStringAsync();

		Assert.True(response.IsSuccessStatusCode,
			$"Status: {response.StatusCode}, Body: {body}");
	}

	[Fact]
	public async Task McpEndpoint_ListTools_ContainsListApis()
	{
		var client = CreateFactory().CreateClient();

		var response = await client.SendAsync(McpRequest(new
		{
			jsonrpc = "2.0",
			id = 2,
			method = "tools/list",
			@params = new { },
		}));
		var body = await response.Content.ReadAsStringAsync();

		Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode}, Body: {body}");
		Assert.Contains("list_apis", body);
		Assert.Contains("search_apis", body);
	}

	[Fact]
	public async Task McpEndpoint_ResourcesTemplatesList_ReturnsSuccess()
	{
		var client = CreateFactory().CreateClient();

		var response = await client.SendAsync(McpRequest(new
		{
			jsonrpc = "2.0",
			id = 2,
			method = "resources/templates/list",
			@params = new { },
		}));
		var body = await response.Content.ReadAsStringAsync();

		Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode}, Body: {body}");
	}

	[Fact]
	public async Task McpEndpoint_CallListApis_ReturnsContracts()
	{
		var client = CreateFactory().CreateClient();

		var response = await client.SendAsync(McpRequest(new
		{
			jsonrpc = "2.0",
			id = 3,
			method = "tools/call",
			@params = new { name = "list_apis", arguments = new { } },
		}));
		var body = await response.Content.ReadAsStringAsync();

		Assert.True(response.IsSuccessStatusCode, $"Status: {response.StatusCode}, Body: {body}");
		Assert.Contains("Service1.yaml", body);
	}
}

internal class MockWikiApiHandler : HttpMessageHandler
{
	private readonly string _contractsJson;
	private readonly string _contractYaml;

	public MockWikiApiHandler(string contractsJson, string contractYaml)
	{
		_contractsJson = contractsJson;
		_contractYaml = contractYaml;
	}

	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var path = request.RequestUri?.PathAndQuery ?? "";

		if (path.StartsWith("/api/contracts/search"))
		{
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(_contractsJson, Encoding.UTF8, "application/json"),
			});
		}

		if (path.StartsWith("/api/contracts/") && path.Length > "/api/contracts/".Length)
		{
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(_contractYaml, Encoding.UTF8, "application/yaml"),
			});
		}

		if (path == "/api/contracts")
		{
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent(_contractsJson, Encoding.UTF8, "application/json"),
			});
		}

		return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
	}
}
