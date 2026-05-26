## Зачем

Запросы к MCP-серверу и wiki-api не логируются, что делает невозможным анализ поведения системы после прогона тестовых сценариев. Логирование необходимо для сбора отчёта по результатам E2E-тестирования.

## Что меняется

- Добавляется структурированное логирование в `ApiDiscoveryTools` (MCP-сервер): имя инструмента, параметры, статус
- Добавляется структурированное логирование в `ApiContractResources` (MCP-сервер): имя ресурса, параметры, статус
- Добавляется структурированное логирование в `ContractsController` (wiki-api): эндпоинт, параметры, HTTP-статус ответа

## Возможности

### Новые возможности

- `mcp-request-logging`: логирование вызовов инструментов и ресурсов на стороне MCP-сервера
- `wiki-api-request-logging`: логирование входящих HTTP-запросов на стороне wiki-api

### Изменённые возможности

- `mcp-tools-api-discovery`: добавляется логирование в существующие инструменты `list_apis` и `search_apis`
- `mcp-resources-api-contract`: добавляется логирование в существующий ресурс `api://{name}`

## Влияние

- `apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs` — добавляется `ILogger<ApiDiscoveryTools>`
- `apps/mcp/src/Mcp.Api/Resources/ApiContractResources.cs` — добавляется `ILogger<ApiContractResources>`
- `apps/wiki/src/Wiki.Api/Controllers/ContractsController.cs` — добавляется `ILogger<ContractsController>`
- Логи пишутся в stdout, собираются через `docker compose logs` после прогона тестов
