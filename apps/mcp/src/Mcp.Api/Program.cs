using Mcp.Api.Clients;
using Mcp.Api.Resources;
using Mcp.Api.Tools;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg
	.MinimumLevel.Information()
	.Enrich.WithProperty("service", "MCP")
	.WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3} [{service}] {Message:lj} {Properties:j}{NewLine}{Exception}"));

var wikiApiUrl = builder.Configuration["WIKI_API_URL"]
	?? Environment.GetEnvironmentVariable("WIKI_API_URL")
	?? "http://localhost:5000";

builder.Services.AddHttpClient<IWikiApiClient, WikiApiClient>(client =>
{
	client.BaseAddress = new Uri(wikiApiUrl);
});

builder.Services
	.AddMcpServer()
	.WithHttpTransport(options => options.Stateless = true)
	.WithTools<ApiDiscoveryTools>()
	.WithResources<ApiContractResources>();

var app = builder.Build();

app.MapMcp();

app.Run();

public partial class Program { }
