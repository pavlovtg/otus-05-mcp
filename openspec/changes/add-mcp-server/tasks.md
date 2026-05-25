## 1. Инфраструктура проекта

- [ ] 1.1 Создать структуру директорий `apps/mcp/src/Mcp.Api/` и `apps/mcp/tests/Mcp.Api.Tests/`
- [ ] 1.2 Создать .NET 10 проект `Mcp.Api.csproj` с зависимостями `ModelContextProtocol` и `Microsoft.Extensions.Http`
- [ ] 1.3 Создать тестовый проект `Mcp.Api.Tests.csproj` с зависимостями xUnit, Moq, `Microsoft.AspNetCore.Mvc.Testing`, coverlet
- [ ] 1.4 Настроить `Program.cs`: регистрация MCP-сервера с HTTP-транспортом, DI, конфигурация `WIKI_API_URL`

## 2. HTTP-клиент wiki-api

- [ ] 2.1 Создать модель `ContractInfo` (name, title, version, description)
- [ ] 2.2 Создать `Clients/WikiApiClient.cs` — типизированный HttpClient
- [ ] 2.3 Реализовать методы: `GetContractsAsync()`, `SearchContractsAsync(query)`, `GetContractAsync(name)`
- [ ] 2.4 Реализовать обработку ошибок (недоступность wiki-api → понятное сообщение)
- [ ] 2.5 Зарегистрировать `WikiApiClient` в DI

## 3. MCP Tools

- [ ] 3.1 Создать `Tools/ApiDiscoveryTools.cs` с атрибутом `[McpServerToolType]`
- [ ] 3.2 Реализовать tool `list_apis` — возвращает массив `ContractInfo` из wiki-api
- [ ] 3.3 Реализовать tool `search_apis` с параметром `query` — возвращает отфильтрованный массив

## 4. MCP Resources

- [ ] 4.1 Создать `Resources/ApiContractResources.cs` с атрибутом `[McpServerResourceType]`
- [ ] 4.2 Реализовать resource `api://{name}` — возвращает Swagger YAML из wiki-api
- [ ] 4.3 Реализовать `resources/list` — возвращает список всех контрактов как MCP-ресурсы

## 5. .NET-тесты

- [ ] 5.1 Unit-тесты `ApiDiscoveryTools`: `list_apis` и `search_apis` с замоканным `WikiApiClient`
- [ ] 5.2 Unit-тесты `ApiContractResources`: resource `api://{name}` с замоканным `WikiApiClient`
- [ ] 5.3 Integration-тесты `WikiApiClient` с mock HTTP-сервером (`MockHttpMessageHandler`)
- [ ] 5.4 API-тесты через `WebApplicationFactory`: вызов tool `list_apis`, `search_apis`, resource `api://{name}`
- [ ] 5.5 Проверить покрытие ≥ 80%

## 6. E2E-тесты (pytest)

- [ ] 6.1 Создать директорию `/tests/mcp/` с `requirements.txt` (pytest, httpx или mcp-клиент)
- [ ] 6.2 Написать тест `test_list_apis` — вызов tool `list_apis`, проверка структуры ответа
- [ ] 6.3 Написать тест `test_search_apis` — вызов tool `search_apis` с параметром, проверка фильтрации
- [ ] 6.4 Написать тест `test_resources_list` — запрос `resources/list`, проверка наличия ресурсов
- [ ] 6.5 Написать тест `test_resource_get` — запрос `api://{name}`, проверка содержимого YAML
- [ ] 6.6 Добавить `README.md` в `/tests/mcp/` с инструкцией запуска

## 7. Docker и деплой

- [ ] 7.1 Создать `apps/mcp/Dockerfile` для сборки образа mcp
- [ ] 7.2 Добавить сервис `mcp` в `infrastructure/docker-compose/docker-compose.yaml` (порт 5200, зависимость от wiki-api, переменная `WIKI_API_URL`)

## 8. GitHub Actions CI

- [ ] 8.1 Убедиться что `.github/workflows/dotnet-ci.yml` подхватывает новый проект `apps/mcp/`
- [ ] 8.2 Создать workflow `.github/workflows/mcp-e2e.yml`: поднять docker-compose, запустить pytest из `/tests/mcp/`

## 9. ADR

- [ ] 9.1 Создать ADR-0010 (стек MCP-сервера: .NET 10, ModelContextProtocol SDK, Streamable HTTP) в `docs/adr/dotnet/`
- [ ] 9.2 Создать ADR-0011 (e2e-тесты через pytest, отдельный CI workflow) в `docs/adr/general/`
- [ ] 9.3 Создать ADR-0012 (Python code style) в `docs/adr/general/`
- [ ] 9.4 Добавить записи ADR-0010, ADR-0011, ADR-0012 в `docs/adr/README.md`
