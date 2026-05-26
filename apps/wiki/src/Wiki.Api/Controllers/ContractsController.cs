using Microsoft.AspNetCore.Mvc;
using Wiki.Api.Services;

namespace Wiki.Api.Controllers;

[ApiController]
[Route("api/contracts")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;
    private readonly ILogger<ContractsController> _logger;

    public ContractsController(IContractService contractService, ILogger<ContractsController> logger)
    {
        _contractService = contractService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var contracts = _contractService.GetAll();
        _logger.LogInformation("Request handled Endpoint={Endpoint} Params={{}} Status={Status} Count={Count}",
            "GET /api/contracts", 200, contracts.Count());
        return Ok(contracts);
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            _logger.LogWarning("Request handled Endpoint={Endpoint} Params={{q={Q}}} Status={Status}",
                "GET /api/contracts/search", q, 400);
            return BadRequest(new { detail = "Query parameter 'q' is required and cannot be empty." });
        }

        var results = _contractService.Search(q);
        _logger.LogInformation("Request handled Endpoint={Endpoint} Params={{q={Q}}} Status={Status} Count={Count}",
            "GET /api/contracts/search", q, 200, results.Count());
        return Ok(results);
    }

    [HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        if (!_contractService.IsValidName(name))
        {
            _logger.LogWarning("Request handled Endpoint={Endpoint} Params={{name={Name}}} Status={Status}",
                "GET /api/contracts/{name}", name, 400);
            return BadRequest(new { detail = "Invalid file name. Path traversal is not allowed." });
        }

        var content = _contractService.GetContent(name);
        if (content is null)
        {
            _logger.LogWarning("Request handled Endpoint={Endpoint} Params={{name={Name}}} Status={Status}",
                "GET /api/contracts/{name}", name, 404);
            return NotFound(new { detail = $"Contract '{name}' not found." });
        }

        _logger.LogInformation("Request handled Endpoint={Endpoint} Params={{name={Name}}} Status={Status}",
            "GET /api/contracts/{name}", name, 200);
        return Content(content, "application/yaml");
    }
}
