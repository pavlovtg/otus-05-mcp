using System.ComponentModel;
using Mcp.Api.Clients;
using Mcp.Api.Models;
using ModelContextProtocol.Server;

namespace Mcp.Api.Tools;

[McpServerToolType]
public class ApiDiscoveryTools
{
	private readonly IWikiApiClient _wikiApiClient;

	public ApiDiscoveryTools(IWikiApiClient wikiApiClient)
	{
		_wikiApiClient = wikiApiClient;
	}

	[McpServerTool(Name = "list_apis")]
	[Description("Возвращает список всех доступных API внутренних сервисов")]
	public async Task<IReadOnlyList<ContractInfo>> ListApisAsync()
	{
		return await _wikiApiClient.GetContractsAsync();
	}

	[McpServerTool(Name = "search_apis")]
	[Description("Поиск API по имени или описанию")]
	public async Task<IReadOnlyList<ContractInfo>> SearchApisAsync(
		[Description("Строка поиска")] string query)
	{
		return await _wikiApiClient.SearchContractsAsync(query);
	}
}
