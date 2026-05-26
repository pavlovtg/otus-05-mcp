using Mcp.Api.Clients;
using Mcp.Api.Resources;
using Mcp.Api.Tools;

var builder = WebApplication.CreateBuilder(args);

var wikiApiUrl = builder.Configuration["WIKI_API_URL"]
	?? Environment.GetEnvironmentVariable("WIKI_API_URL")
	?? "http://localhost:5000";

builder.Services.AddHttpClient<IWikiApiClient, WikiApiClient>(client =>
{
	client.BaseAddress = new Uri(wikiApiUrl);
});

builder.Services
	.AddMcpServer()
	.WithHttpTransport()
	.WithTools<ApiDiscoveryTools>()
	.WithResources<ApiContractResources>();

var app = builder.Build();

app.MapMcp();

app.Run();

public partial class Program { }
