using System.Net.Http.Json;

namespace Dictionaries.Client;

/// <summary>
/// Клиент для Dictionaries API v1.0.0
/// Базовый URL: http://{host}/api
/// </summary>
public class DictionariesClient
{
	private readonly HttpClient _httpClient;

	public DictionariesClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	/// <summary>
	/// Получить справочник по контекстным метрикам
	/// GET /api/dictionaries/asset/contextMetrics/{metric}
	/// </summary>
	public async Task<IReadOnlyList<DictionaryEntry>> GetContextMetricAsync(ContextMetric metric, CancellationToken cancellationToken = default)
	{
		var result = await _httpClient.GetFromJsonAsync<List<DictionaryEntry>>(
			$"/api/dictionaries/asset/contextMetrics/{metric.ToString().ToLower()}",
			cancellationToken);
		return result ?? [];
	}

	/// <summary>
	/// Получить справочник по значимости
	/// GET /api/importance
	/// </summary>
	public async Task<IReadOnlyList<DictionaryEntry>> GetImportanceAsync(CancellationToken cancellationToken = default)
	{
		var result = await _httpClient.GetFromJsonAsync<List<DictionaryEntry>>(
			"/api/importance",
			cancellationToken);
		return result ?? [];
	}
}

public enum ContextMetric
{
	Ar,
	Cdp,
	Cr,
	Ir,
	Td,
}

public enum Severity
{
	NotDefined,
	None,
	Low,
	BelowMedium,
	Medium,
	AboveMedium,
	High,
}

public class DictionaryEntry
{
	public string Id { get; set; } = string.Empty;
	public Severity Severity { get; set; }
	public string DisplayName { get; set; } = string.Empty;
}
