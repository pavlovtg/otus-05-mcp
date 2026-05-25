using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Wiki.Api.Models;

namespace Wiki.Api.Tests.Api;

public class ContractsApiTests : IClassFixture<ContractsApiTests.WikiApiFactory>, IDisposable
{
    private readonly WikiApiFactory _factory;
    private readonly HttpClient _client;

    public ContractsApiTests(WikiApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public class WikiApiFactory : WebApplicationFactory<Program>, IDisposable
    {
        public string ContractsPath { get; } = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        public WikiApiFactory()
        {
            Directory.CreateDirectory(ContractsPath);
        }

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["CONTRACTS_PATH"] = ContractsPath
                });
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing && Directory.Exists(ContractsPath))
                Directory.Delete(ContractsPath, recursive: true);
        }
    }

    private void WriteYaml(string name, string title, string version = "1.0.0", string description = "Test description")
    {
        var content = $$"""
            openapi: "3.0.3"
            info:
              title: {{title}}
              version: "{{version}}"
              description: {{description}}
            paths: {}
            """;
        File.WriteAllText(Path.Combine(_factory.ContractsPath, name), content);
    }

    [Fact]
    public async Task GetAll_EmptyDirectory_Returns200WithEmptyArray()
    {
        var response = await _client.GetAsync("/api/contracts");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contracts = await response.Content.ReadFromJsonAsync<List<ContractInfo>>();
        Assert.NotNull(contracts);
        Assert.Empty(contracts);
    }

    [Fact]
    public async Task GetAll_WithContracts_Returns200WithList()
    {
        WriteYaml("TestService.yaml", "Test Service API");

        var response = await _client.GetAsync("/api/contracts");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contracts = await response.Content.ReadFromJsonAsync<List<ContractInfo>>();
        Assert.NotNull(contracts);
        Assert.Contains(contracts, c => c.Name == "TestService.yaml" && c.Title == "Test Service API");
    }

    [Fact]
    public async Task GetByName_ExistingFile_Returns200WithYamlContent()
    {
        WriteYaml("MyContract.yaml", "My Contract API");

        var response = await _client.GetAsync("/api/contracts/MyContract.yaml");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/yaml", response.Content.Headers.ContentType?.MediaType);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("My Contract API", content);
    }

    [Fact]
    public async Task GetByName_NonExistentFile_Returns404()
    {
        var response = await _client.GetAsync("/api/contracts/nonexistent.yaml");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetByName_PathTraversal_Returns400()
    {
        var response = await _client.GetAsync("/api/contracts/..%2Fsecret.yaml");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Search_WithResults_Returns200WithMatchingContracts()
    {
        WriteYaml("Assets.yaml", "Assets Processing API");
        WriteYaml("Users.yaml", "Users Management API");

        var response = await _client.GetAsync("/api/contracts/search?q=assets");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contracts = await response.Content.ReadFromJsonAsync<List<ContractInfo>>();
        Assert.NotNull(contracts);
        Assert.Single(contracts);
        Assert.Equal("Assets.yaml", contracts[0].Name);
    }

    [Fact]
    public async Task Search_NoResults_Returns200WithEmptyArray()
    {
        var response = await _client.GetAsync("/api/contracts/search?q=xyznonexistent");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var contracts = await response.Content.ReadFromJsonAsync<List<ContractInfo>>();
        Assert.NotNull(contracts);
        Assert.Empty(contracts);
    }

    [Fact]
    public async Task Search_EmptyQuery_Returns400()
    {
        var response = await _client.GetAsync("/api/contracts/search?q=");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Search_MissingQuery_Returns400()
    {
        var response = await _client.GetAsync("/api/contracts/search");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
