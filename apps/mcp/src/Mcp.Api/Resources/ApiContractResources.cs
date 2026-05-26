using System.Collections;
using System.ComponentModel;
using Mcp.Api.Clients;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace Mcp.Api.Resources;

/// <summary>
/// Динамический список ресурсов MCP для API-контрактов.
/// Реализует IEnumerable&lt;McpServerResource&gt; — SDK регистрирует их как прямые ресурсы (resources/list).
/// </summary>
public class ApiContractResourceCollection : IEnumerable<McpServerResource>
{
	private readonly IWikiApiClient _wikiApiClient;

	public ApiContractResourceCollection(IWikiApiClient wikiApiClient)
	{
		_wikiApiClient = wikiApiClient;
	}

	public IEnumerator<McpServerResource> GetEnumerator()
	{
		var contracts = _wikiApiClient.GetContractsAsync().GetAwaiter().GetResult();

		foreach (var contract in contracts)
		{
			var name = contract.Name;
			var uri = $"api://{name}";

			yield return McpServerResource.Create(
				(IWikiApiClient client) => client.GetContractAsync(name),
				new McpServerResourceCreateOptions
				{
					Name = name,
					Title = contract.Title,
					Description = contract.Description,
					UriTemplate = uri,
					MimeType = "text/yaml",
					Services = null,
				});
		}
	}

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

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
