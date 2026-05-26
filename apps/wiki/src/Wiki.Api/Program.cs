using Serilog;
using Wiki.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg
    .MinimumLevel.Information()
    .Enrich.WithProperty("service", "WikiAPI")
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3} [{service}] {Message:lj} {Properties:j}{NewLine}{Exception}"));

builder.Services.AddControllers();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();
app.MapControllers();

app.Run();

public partial class Program { }
