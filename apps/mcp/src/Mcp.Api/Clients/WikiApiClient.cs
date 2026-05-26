using System.Net.Http.Json;
using Mcp.Api.Models;

namespace Mcp.Api.Clients;

public class WikiApiClient : IWikiApiClient
{
	private readonly HttpClient _httpClient;

	public WikiApiClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IReadOnlyList<ContractInfo>> GetContractsAsync()
	{
		try
		{
			var result = await _httpClient.GetFromJsonAsync<List<ContractInfo>>("/api/contracts");
			return result ?? [];
		}
		catch (HttpRequestException ex)
		{
			throw new WikiApiException("Не удалось получить список контрактов из wiki-api", ex);
		}
	}

	public async Task<IReadOnlyList<ContractInfo>> SearchContractsAsync(string query)
	{
		try
		{
			var result = await _httpClient.GetFromJsonAsync<List<ContractInfo>>(
				$"/api/contracts/search?q={Uri.EscapeDataString(query)}");
			return result ?? [];
		}
		catch (HttpRequestException ex)
		{
			throw new WikiApiException($"Не удалось выполнить поиск контрактов в wiki-api: {query}", ex);
		}
	}

	public async Task<string> GetContractAsync(string name)
	{
		try
		{
			var response = await _httpClient.GetAsync($"/api/contracts/{Uri.EscapeDataString(name)}");
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadAsStringAsync();
		}
		catch (HttpRequestException ex)
		{
			throw new WikiApiException($"Не удалось получить контракт '{name}' из wiki-api", ex);
		}
	}
}
