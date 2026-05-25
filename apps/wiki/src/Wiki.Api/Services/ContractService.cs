using Wiki.Api.Models;
using YamlDotNet.RepresentationModel;

namespace Wiki.Api.Services;

public class ContractService : IContractService
{
    private readonly string _contractsPath;

    public ContractService(IConfiguration configuration)
    {
        _contractsPath = configuration["CONTRACTS_PATH"] ?? "./content";
    }

    public IReadOnlyList<ContractInfo> GetAll()
    {
        if (!Directory.Exists(_contractsPath))
            return [];

        return Directory
            .EnumerateFiles(_contractsPath, "*.yaml")
            .Concat(Directory.EnumerateFiles(_contractsPath, "*.yml"))
            .Select(ParseContractInfo)
            .ToList();
    }

    public string? GetContent(string name)
    {
        var filePath = Path.Combine(_contractsPath, name);
        if (!File.Exists(filePath))
            return null;

        return File.ReadAllText(filePath);
    }

    public IReadOnlyList<ContractInfo> Search(string query)
    {
        var lowerQuery = query.ToLowerInvariant();
        return GetAll()
            .Where(c =>
                c.Name.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
                c.Title.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase) ||
                c.Description.Contains(lowerQuery, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Только имя файла без пути
        if (name.Contains('/') || name.Contains('\\') || name.Contains(".."))
            return false;

        // Проверяем что результирующий путь внутри CONTRACTS_PATH
        var fullPath = Path.GetFullPath(Path.Combine(_contractsPath, name));
        var basePath = Path.GetFullPath(_contractsPath);
        return fullPath.StartsWith(basePath + Path.DirectorySeparatorChar) ||
               fullPath == basePath;
    }

    private ContractInfo ParseContractInfo(string filePath)
    {
        var name = Path.GetFileName(filePath);
        try
        {
            var content = File.ReadAllText(filePath);
            var yaml = new YamlStream();
            yaml.Load(new StringReader(content));

            if (yaml.Documents.Count == 0)
                return new ContractInfo(name, name, string.Empty, string.Empty);

            var root = (YamlMappingNode)yaml.Documents[0].RootNode;

            if (!root.Children.TryGetValue(new YamlScalarNode("info"), out var infoNode) ||
                infoNode is not YamlMappingNode info)
                return new ContractInfo(name, name, string.Empty, string.Empty);

            var title = GetScalarValue(info, "title");
            var version = GetScalarValue(info, "version");
            var description = GetScalarValue(info, "description");

            return new ContractInfo(name, title, version, description);
        }
        catch
        {
            return new ContractInfo(name, name, string.Empty, string.Empty);
        }
    }

    private static string GetScalarValue(YamlMappingNode node, string key)
    {
        if (node.Children.TryGetValue(new YamlScalarNode(key), out var value) &&
            value is YamlScalarNode scalar)
            return scalar.Value ?? string.Empty;
        return string.Empty;
    }
}
