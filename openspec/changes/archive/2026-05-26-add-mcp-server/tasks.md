## 1. Инфраструктура проекта

- [x] 1.1 Создать структуру директорий `apps/mcp/src/Mcp.Api/` и `apps/mcp/tests/Mcp.Api.Tests/`
- [x] 1.2 Создать .NET 10 проект `Mcp.Api.csproj` с зависимостями `ModelContextProtocol` и `Microsoft.Extensions.Http`
- [x] 1.3 Создать тестовый проект `Mcp.Api.Tests.csproj` с зависимостями xUnit, Moq, `Microsoft.AspNetCore.Mvc.Testing`, coverlet
- [x] 1.4 Настроить `Program.cs`: регистрация MCP-сервера с HTTP-транспортом, DI, конфигурация `WIKI_API_URL`

## 2. HTTP-клиент wiki-api

- [x] 2.1 Создать модель `ContractInfo` (name, title, version, description)
- [x] 2.2 Создать `Clients/WikiApiClient.cs` — типизированный HttpClient
- [x] 2.3 Реализовать методы: `GetContractsAsync()`, `SearchContractsAsync(query)`, `GetContractAsync(name)`
- [x] 2.4 Реализовать обработку ошибок (недоступность wiki-api → понятное сообщение)
- [x] 2.5 Зарегистрировать `WikiApiClient` в DI

## 3. MCP Tools

- [x] 3.1 Создать `Tools/ApiDiscoveryTools.cs` с атрибутом `[McpServerToolType]`
- [x] 3.2 Реализовать tool `list_apis` — возвращает массив `ContractInfo` из wiki-api
- [x] 3.3 Реализовать tool `search_apis` с параметром `query` — возвращает отфильтрованный массив

## 4. MCP Resources

- [x] 4.1 Создать `Resources/ApiContractResources.cs` с атрибутом `[McpServerResourceType]`
- [x] 4.2 Реализовать resource `api://{name}` — возвращает Swagger YAML из wiki-api
- [x] 4.3 Реализовать `resources/templates/list` — возвращает список всех контрактов как MCP-ресурсы

## 5. .NET-тесты

- [x] 5.1 Unit-тесты `ApiDiscoveryTools`: `list_apis` и `search_apis` с замоканным `WikiApiClient`
- [x] 5.2 Unit-тесты `ApiContractResources`: resource `api://{name}` с замоканным `WikiApiClient`
- [x] 5.3 Integration-тесты `WikiApiClient` с mock HTTP-сервером (`MockHttpMessageHandler`)
- [x] 5.4 API-тесты через `WebApplicationFactory`: вызов tool `list_apis`, `search_apis`, resource `api://{name}`
- [x] 5.5 Проверить покрытие ≥ 80%

## 6. E2E-тесты (pytest)

- [x] 6.1 Создать директорию `/tests/mcp/` с `requirements.txt` (pytest, httpx или mcp-клиент)
- [x] 6.2 Написать тест `test_list_apis` — вызов tool `list_apis`, проверка структуры ответа
- [x] 6.3 Написать тест `test_search_apis` — вызов tool `search_apis` с параметром, проверка фильтрации
- [x] 6.4 Написать тест `test_resources_list` — запрос `resources/list`, проверка наличия ресурсов
- [x] 6.5 Написать тест `test_resource_get` — запрос `api://{name}`, проверка содержимого YAML
- [x] 6.6 Добавить `README.md` в `/tests/mcp/` с инструкцией запуска

## 7. Docker и деплой

- [x] 7.1 Создать `apps/mcp/Dockerfile` для сборки образа mcp
- [x] 7.2 Добавить сервис `mcp` в `infrastructure/docker-compose/docker-compose.yaml` (порт 5200, зависимость от wiki-api, переменная `WIKI_API_URL`)

## 8. GitHub Actions CI

- [x] 8.1 Убедиться что `.github/workflows/dotnet-ci.yml` подхватывает новый проект `apps/mcp/`
- [x] 8.2 Создать workflow `.github/workflows/mcp-e2e.yml`: поднять docker-compose, запустить pytest из `/tests/mcp/`

## 9. ADR

- [x] 9.1 Создать ADR-0010 (стек MCP-сервера: .NET 10, ModelContextProtocol SDK, Streamable HTTP) в `docs/adr/dotnet/`
- [x] 9.2 Создать ADR-0011 (e2e-тесты через pytest, отдельный CI workflow) в `docs/adr/general/`
- [x] 9.3 Создать ADR-0012 (Python code style) в `docs/adr/general/`
- [x] 9.4 Добавить записи ADR-0010, ADR-0011, ADR-0012 в `docs/adr/README.md`
