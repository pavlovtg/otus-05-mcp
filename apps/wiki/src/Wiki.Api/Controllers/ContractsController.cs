using Microsoft.AspNetCore.Mvc;
using Wiki.Api.Services;

namespace Wiki.Api.Controllers;

[ApiController]
[Route("api/contracts")]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractsController(IContractService contractService)
    {
        _contractService = contractService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var contracts = _contractService.GetAll();
        return Ok(contracts);
    }

    [HttpGet("search")]
    public IActionResult Search([FromQuery] string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { detail = "Query parameter 'q' is required and cannot be empty." });

        var results = _contractService.Search(q);
        return Ok(results);
    }

    [HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        if (!_contractService.IsValidName(name))
            return BadRequest(new { detail = "Invalid file name. Path traversal is not allowed." });

        var content = _contractService.GetContent(name);
        if (content is null)
            return NotFound(new { detail = $"Contract '{name}' not found." });

        return Content(content, "application/yaml");
    }
}
