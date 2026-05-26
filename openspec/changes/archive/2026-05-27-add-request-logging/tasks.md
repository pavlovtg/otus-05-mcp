## 1. Подготовка зависимостей

- [x] 1.1 Добавить пакет `Serilog.AspNetCore` в `apps/mcp/src/Mcp.Api/Mcp.Api.csproj`
- [x] 1.2 Добавить пакет `Serilog.AspNetCore` в `apps/wiki/src/Wiki.Api/Wiki.Api.csproj`

## 2. Настройка Serilog в MCP-сервере

- [x] 2.1 Подключить Serilog в `apps/mcp/src/Mcp.Api/Program.cs` через `UseSerilog()` с `outputTemplate` по ADR-0013 и `Enrich.WithProperty("service", "MCP")`
- [x] 2.2 Добавить `ILogger<ApiDiscoveryTools>` в конструктор `ApiDiscoveryTools`
- [x] 2.3 Добавить логирование в метод `ListApisAsync`: `Tool=list_apis`, `Status=success/error`
- [x] 2.4 Добавить логирование в метод `SearchApisAsync`: `Tool=search_apis`, `Params={query}`, `Status=success/error`
- [x] 2.5 Добавить `ILogger<ApiContractResources>` в конструктор `ApiContractResources`
- [x] 2.6 Добавить логирование в метод `GetApiContractAsync`: `Resource=api://{name}`, `Params={name}`, `Status=success/error`

## 3. Настройка Serilog в wiki-api

- [x] 3.1 Подключить Serilog в `apps/wiki/src/Wiki.Api/Program.cs` через `UseSerilog()` с `outputTemplate` по ADR-0013 и `Enrich.WithProperty("service", "WikiAPI")`
- [x] 3.2 Добавить `ILogger<ContractsController>` в конструктор `ContractsController`
- [x] 3.3 Добавить логирование в метод `GetAll`: `Endpoint=GET /api/contracts`, `Status=200`
- [x] 3.4 Добавить логирование в метод `Search`: `Endpoint=GET /api/contracts/search`, `Params={q}`, `Status=200/400`
- [x] 3.5 Добавить логирование в метод `GetByName`: `Endpoint=GET /api/contracts/{name}`, `Params={name}`, `Status=200/400/404`

## 4. Проверка

- [x] 4.1 Собрать оба сервиса: `dotnet build`
- [x] 4.2 Запустить сервисы через `docker compose up -d` и убедиться, что логи пишутся в нужном формате
- [x] 4.3 Прогнать E2E-тесты: `pytest tests/mcp -v` и проверить логи через `docker compose logs`
