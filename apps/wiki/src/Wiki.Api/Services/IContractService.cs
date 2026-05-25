using Wiki.Api.Models;

namespace Wiki.Api.Services;

public interface IContractService
{
    IReadOnlyList<ContractInfo> GetAll();
    string? GetContent(string name);
    IReadOnlyList<ContractInfo> Search(string query);
    bool IsValidName(string name);
}
