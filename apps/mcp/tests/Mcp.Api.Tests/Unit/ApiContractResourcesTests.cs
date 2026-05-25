using Mcp.Api.Clients;
using Mcp.Api.Resources;
using Moq;

namespace Mcp.Api.Tests.Unit;

public class ApiContractResourcesTests
{
	private readonly Mock<IWikiApiClient> _wikiClientMock;
	private readonly ApiContractResources _resources;

	public ApiContractResourcesTests()
	{
		_wikiClientMock = new Mock<IWikiApiClient>(MockBehavior.Strict);
		_resources = new ApiContractResources(_wikiClientMock.Object);
	}

	[Fact]
	public async Task GetApiContract_ReturnsYamlContent()
	{
		var yaml = "openapi: 3.0.0\ninfo:\n  title: Test API";
		_wikiClientMock.Setup(c => c.GetContractAsync("Service1.yaml")).ReturnsAsync(yaml);

		var result = await _resources.GetApiContractAsync("Service1.yaml");

		Assert.Equal(yaml, result);
		_wikiClientMock.Verify(c => c.GetContractAsync("Service1.yaml"), Times.Once);
	}

	[Fact]
	public async Task GetApiContract_WhenNotFound_ThrowsWikiApiException()
	{
		_wikiClientMock.Setup(c => c.GetContractAsync("nonexistent.yaml"))
			.ThrowsAsync(new WikiApiException("Не удалось получить контракт 'nonexistent.yaml' из wiki-api"));

		await Assert.ThrowsAsync<WikiApiException>(
			() => _resources.GetApiContractAsync("nonexistent.yaml"));
	}
}
