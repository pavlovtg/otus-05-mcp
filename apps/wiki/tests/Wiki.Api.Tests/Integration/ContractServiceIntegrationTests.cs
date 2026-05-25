using Microsoft.Extensions.Configuration;
using Wiki.Api.Services;

namespace Wiki.Api.Tests.Integration;

public class ContractServiceIntegrationTests : IDisposable
{
    private readonly string _tempDir;
    private readonly ContractService _service;

    public ContractServiceIntegrationTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDir);

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["CONTRACTS_PATH"] = _tempDir })
            .Build();
        _service = new ContractService(config);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }

    private void WriteYaml(string name, string title, string version = "1.0.0", string description = "")
    {
        var content = $$"""
            openapi: "3.0.3"
            info:
              title: {{title}}
              version: "{{version}}"
              description: {{description}}
            paths: {}
            """;
        File.WriteAllText(Path.Combine(_tempDir, name), content);
    }

    [Fact]
    public void GetAll_EmptyDirectory_ReturnsEmptyList()
    {
        var result = _service.GetAll();
        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_WithYamlFiles_ReturnsContracts()
    {
        WriteYaml("Service1.yaml", "Service One");
        WriteYaml("Service2.yaml", "Service Two");

        var result = _service.GetAll();

        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "Service1.yaml" && c.Title == "Service One");
        Assert.Contains(result, c => c.Name == "Service2.yaml" && c.Title == "Service Two");
    }

    [Fact]
    public void GetAll_ParsesVersionAndDescription()
    {
        WriteYaml("Service.yaml", "My Service", "2.0.0", "A great service");

        var result = _service.GetAll();

        var contract = Assert.Single(result);
        Assert.Equal("2.0.0", contract.Version);
        Assert.Equal("A great service", contract.Description);
    }

    [Fact]
    public void GetContent_ExistingFile_ReturnsContent()
    {
        WriteYaml("Service.yaml", "My Service");

        var content = _service.GetContent("Service.yaml");

        Assert.NotNull(content);
        Assert.Contains("My Service", content);
    }

    [Fact]
    public void GetContent_NonExistentFile_ReturnsNull()
    {
        var content = _service.GetContent("nonexistent.yaml");
        Assert.Null(content);
    }

    [Fact]
    public void Search_ByTitle_ReturnsMatchingContracts()
    {
        WriteYaml("Assets.yaml", "Assets Processing API");
        WriteYaml("Users.yaml", "Users Management API");

        var result = _service.Search("assets");

        Assert.Single(result);
        Assert.Equal("Assets.yaml", result[0].Name);
    }

    [Fact]
    public void Search_ByName_ReturnsMatchingContracts()
    {
        WriteYaml("EventStream.yaml", "Event Stream Service");
        WriteYaml("Users.yaml", "Users API");

        var result = _service.Search("EventStream");

        Assert.Single(result);
        Assert.Equal("EventStream.yaml", result[0].Name);
    }

    [Fact]
    public void Search_ByDescription_ReturnsMatchingContracts()
    {
        WriteYaml("Service.yaml", "My Service", "1.0.0", "Handles asset processing");
        WriteYaml("Other.yaml", "Other Service", "1.0.0", "Handles user management");

        var result = _service.Search("asset processing");

        Assert.Single(result);
        Assert.Equal("Service.yaml", result[0].Name);
    }

    [Fact]
    public void Search_NoMatch_ReturnsEmptyList()
    {
        WriteYaml("Service.yaml", "My Service");

        var result = _service.Search("xyznonexistent");

        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_DynamicUpdate_ReflectsNewFile()
    {
        var initial = _service.GetAll();
        Assert.Empty(initial);

        WriteYaml("NewService.yaml", "New Service");

        var updated = _service.GetAll();
        Assert.Single(updated);
    }
}
