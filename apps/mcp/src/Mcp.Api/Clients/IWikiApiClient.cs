using Mcp.Api.Models;

namespace Mcp.Api.Clients;

public interface IWikiApiClient
{
	Task<IReadOnlyList<ContractInfo>> GetContractsAsync();
	Task<IReadOnlyList<ContractInfo>> SearchContractsAsync(string query);
	Task<string> GetContractAsync(string name);
}
