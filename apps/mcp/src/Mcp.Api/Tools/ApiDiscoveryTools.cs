using System.ComponentModel;
using Mcp.Api.Clients;
using Mcp.Api.Models;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace Mcp.Api.Tools;

[McpServerToolType]
public class ApiDiscoveryTools
{
	private readonly IWikiApiClient _wikiApiClient;
	private readonly ILogger<ApiDiscoveryTools> _logger;

	public ApiDiscoveryTools(IWikiApiClient wikiApiClient, ILogger<ApiDiscoveryTools> logger)
	{
		_wikiApiClient = wikiApiClient;
		_logger = logger;
	}

	[McpServerTool(Name = "list_apis")]
	[Description("Возвращает список всех доступных API внутренних сервисов")]
	public async Task<IReadOnlyList<ContractInfo>> ListApisAsync()
	{
		try
		{
			var result = await _wikiApiClient.GetContractsAsync();
			_logger.LogInformation("Tool invoked Tool={Tool} Params={{}} Status=success Count={Count}",
				"list_apis", result.Count);
			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Tool failed Tool={Tool} Params={{}} Status=error Error={Error}",
				"list_apis", ex.GetType().Name);
			throw;
		}
	}

	[McpServerTool(Name = "search_apis")]
	[Description("Поиск API по имени или описанию")]
	public async Task<IReadOnlyList<ContractInfo>> SearchApisAsync(
		[Description("Строка поиска")] string query)
	{
		try
		{
			IReadOnlyList<ContractInfo> result;
			if (string.IsNullOrWhiteSpace(query))
				result = await _wikiApiClient.GetContractsAsync();
			else
				result = await _wikiApiClient.SearchContractsAsync(query);

			_logger.LogInformation("Tool invoked Tool={Tool} Params={{query={Query}}} Status=success Count={Count}",
				"search_apis", query, result.Count);
			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Tool failed Tool={Tool} Params={{query={Query}}} Status=error Error={Error}",
				"search_apis", query, ex.GetType().Name);
			throw;
		}
	}
}
