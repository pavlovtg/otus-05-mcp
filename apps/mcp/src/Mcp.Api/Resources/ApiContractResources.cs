using System.ComponentModel;
using Mcp.Api.Clients;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;

namespace Mcp.Api.Resources;

[McpServerResourceType]
public class ApiContractResources
{
	private readonly IWikiApiClient _wikiApiClient;
	private readonly ILogger<ApiContractResources> _logger;

	public ApiContractResources(IWikiApiClient wikiApiClient, ILogger<ApiContractResources> logger)
	{
		_wikiApiClient = wikiApiClient;
		_logger = logger;
	}

	[McpServerResource(UriTemplate = "api://{name}")]
	[Description("Возвращает полное содержимое Swagger YAML-файла для указанного API")]
	public async Task<string> GetApiContractAsync(string name)
	{
		try
		{
			var result = await _wikiApiClient.GetContractAsync(name);
			_logger.LogInformation("Resource requested Resource={Resource} Params={{name={Name}}} Status=success",
				$"api://{name}", name);
			return result;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Resource failed Resource={Resource} Params={{name={Name}}} Status=error Error={Error}",
				$"api://{name}", name, ex.GetType().Name);
			throw;
		}
	}
}
