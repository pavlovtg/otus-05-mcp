using Microsoft.Extensions.Configuration;
using Wiki.Api.Services;

namespace Wiki.Api.Tests.Unit;

public class ContractServiceUnitTests
{
    private static ContractService CreateService(string path)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["CONTRACTS_PATH"] = path })
            .Build();
        return new ContractService(config);
    }

    [Fact]
    public void IsValidName_ValidName_ReturnsTrue()
    {
        var service = CreateService("/tmp/contracts");
        Assert.True(service.IsValidName("MyService.yaml"));
    }

    [Fact]
    public void IsValidName_PathTraversalDotDot_ReturnsFalse()
    {
        var service = CreateService("/tmp/contracts");
        Assert.False(service.IsValidName("../secret.yaml"));
    }

    [Fact]
    public void IsValidName_PathTraversalWithSlash_ReturnsFalse()
    {
        var service = CreateService("/tmp/contracts");
        Assert.False(service.IsValidName("subdir/file.yaml"));
    }

    [Fact]
    public void IsValidName_PathTraversalWithBackslash_ReturnsFalse()
    {
        var service = CreateService("/tmp/contracts");
        Assert.False(service.IsValidName("subdir\\file.yaml"));
    }

    [Fact]
    public void IsValidName_EmptyName_ReturnsFalse()
    {
        var service = CreateService("/tmp/contracts");
        Assert.False(service.IsValidName(""));
    }

    [Fact]
    public void IsValidName_WhitespaceName_ReturnsFalse()
    {
        var service = CreateService("/tmp/contracts");
        Assert.False(service.IsValidName("   "));
    }

    [Fact]
    public void GetAll_NonExistentDirectory_ReturnsEmptyList()
    {
        var service = CreateService("/nonexistent/path/that/does/not/exist");
        var result = service.GetAll();
        Assert.Empty(result);
    }

    [Fact]
    public void Search_EmptyQuery_ReturnsEmpty()
    {
        var service = CreateService("/nonexistent/path");
        var result = service.Search("nonexistent_query_xyz");
        Assert.Empty(result);
    }
}
