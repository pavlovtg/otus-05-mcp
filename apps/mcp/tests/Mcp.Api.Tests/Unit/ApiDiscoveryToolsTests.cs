using Mcp.Api.Clients;
using Mcp.Api.Models;
using Mcp.Api.Tools;
using Moq;

namespace Mcp.Api.Tests.Unit;

public class ApiDiscoveryToolsTests
{
	private readonly Mock<IWikiApiClient> _wikiClientMock;
	private readonly ApiDiscoveryTools _tools;

	public ApiDiscoveryToolsTests()
	{
		_wikiClientMock = new Mock<IWikiApiClient>(MockBehavior.Strict);
		_tools = new ApiDiscoveryTools(_wikiClientMock.Object);
	}

	[Fact]
	public async Task ListApis_ReturnsAllContracts()
	{
		var contracts = new List<ContractInfo>
		{
			new() { Name = "Service1.yaml", Title = "Service 1", Version = "1.0", Description = "Desc 1" },
			new() { Name = "Service2.yaml", Title = "Service 2", Version = "2.0", Description = "Desc 2" },
		};
		_wikiClientMock.Setup(c => c.GetContractsAsync()).ReturnsAsync(contracts);

		var result = await _tools.ListApisAsync();

		Assert.Equal(2, result.Count);
		Assert.Equal("Service1.yaml", result[0].Name);
	}

	[Fact]
	public async Task ListApis_WhenEmpty_ReturnsEmptyList()
	{
		_wikiClientMock.Setup(c => c.GetContractsAsync()).ReturnsAsync([]);

		var result = await _tools.ListApisAsync();

		Assert.Empty(result);
	}

	[Fact]
	public async Task SearchApis_CallsSearchWithQuery()
	{
		var contracts = new List<ContractInfo>
		{
			new() { Name = "Analytics.yaml", Title = "Analytics API", Version = "1.0", Description = "Analytics" },
		};
		_wikiClientMock.Setup(c => c.SearchContractsAsync("analytics")).ReturnsAsync(contracts);

		var result = await _tools.SearchApisAsync("analytics");

		Assert.Single(result);
		Assert.Equal("Analytics.yaml", result[0].Name);
		_wikiClientMock.Verify(c => c.SearchContractsAsync("analytics"), Times.Once);
	}

	[Fact]
	public async Task SearchApis_WhenNoResults_ReturnsEmptyList()
	{
		_wikiClientMock.Setup(c => c.SearchContractsAsync("nonexistent")).ReturnsAsync([]);

		var result = await _tools.SearchApisAsync("nonexistent");

		Assert.Empty(result);
	}
}
