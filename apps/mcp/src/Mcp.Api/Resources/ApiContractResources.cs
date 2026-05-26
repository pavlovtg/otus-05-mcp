using System.ComponentModel;
using Mcp.Api.Clients;
using ModelContextProtocol.Server;

namespace Mcp.Api.Resources;

[McpServerResourceType]
public class ApiContractResources
{
	private readonly IWikiApiClient _wikiApiClient;

	public ApiContractResources(IWikiApiClient wikiApiClient)
	{
		_wikiApiClient = wikiApiClient;
	}

	[McpServerResource(UriTemplate = "api://{name}")]
	[Description("Возвращает полное содержимое Swagger YAML-файла для указанного API")]
	public async Task<string> GetApiContractAsync(string name)
	{
		return await _wikiApiClient.GetContractAsync(name);
	}
}
