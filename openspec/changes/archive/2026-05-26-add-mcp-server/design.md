## Контекст

В репозитории уже есть wiki-api — сервис, предоставляющий каталог Swagger-контрактов внутренних сервисов через REST API. AI-агенты в IDE не имеют прямого доступа к этим контрактам, что затрудняет генерацию кода интеграций.

MCP (Model Context Protocol) — стандартный протокол для предоставления контекста AI-агентам. Добавление MCP-сервера позволит агентам обнаруживать и читать контракты через стандартный интерфейс.

## Цели / Не-цели

**Цели:**

- Реализовать MCP-сервер `apps/mcp/` на .NET 10, использующий wiki-api как источник данных
- Предоставить tools `list_apis` и `search_apis` для обнаружения API
- Предоставить resource `api://{name}` для получения полного Swagger YAML
- Добавить сервис `mcp` в существующий docker-compose
- Покрыть сервис тестами трёх уровней (.NET) + e2e через pytest
- Запускать все тесты в GitHub Actions CI

**Не-цели:**

- Не реализовывать собственное хранилище контрактов (используется wiki-api)
- Не изменять wiki-api
- Не добавлять аутентификацию/авторизацию в MCP-сервер

## Решения

### Транспорт: Streamable HTTP

MCP поддерживает два транспорта: stdio и Streamable HTTP. Выбран HTTP, так как:
- Сервер запускается как отдельный процесс/контейнер, а не дочерний процесс IDE
- HTTP позволяет подключаться нескольким клиентам одновременно
- Упрощает деплой через Docker

Альтернатива stdio отклонена: требует запуска сервера как дочернего процесса IDE, что несовместимо с Docker-деплоем.

Фиксируется в **ADR-0010** (стек MCP-сервера).

### MCP SDK: ModelContextProtocol для .NET

Используется официальный пакет `ModelContextProtocol` (Microsoft). Он предоставляет:
- Регистрацию tools через атрибуты `[McpServerToolType]` / `[McpServerTool]`
- Регистрацию resources через `[McpServerResourceType]` / `[McpServerResource]`
- Интеграцию с ASP.NET Core через `AddMcpServer().WithHttpTransport()`

Альтернатива — реализация протокола вручную — отклонена как избыточная сложность.

Фиксируется в **ADR-0010** (стек MCP-сервера).

### Интеграция с wiki-api: типизированный HttpClient

Для обращений к wiki-api используется типизированный `HttpClient`, зарегистрированный через DI. Базовый URL конфигурируется через `WIKI_API_URL`.

Альтернатива — прямое чтение файлов контрактов — отклонена: нарушает принцип единственного источника данных и дублирует логику wiki-api.

### Структура проекта

Следует ADR-0007 (структура .NET проектов):

```
apps/mcp/
  src/
    Mcp.Api/
      Tools/          # MCP tools
      Resources/      # MCP resources
      Clients/        # HTTP-клиент wiki-api
      Program.cs
      Mcp.Api.csproj
  tests/
    Mcp.Api.Tests/
      Unit/
      Integration/
      Api/
      Mcp.Api.Tests.csproj
  Dockerfile
```

### Тестирование .NET (unit, integration, API)

Следует ADR-0008 (стратегия тестирования):
- **Unit**: tools и resources тестируются с замоканным `WikiApiClient`
- **Integration**: `WikiApiClient` тестируется с mock HTTP-сервером (`MockHttpMessageHandler`)
- **API**: полные MCP-запросы через `WebApplicationFactory<TProgram>`

Запускаются в существующем workflow `.github/workflows/dotnet-ci.yml` (ADR-0009) — он уже покрывает все .NET сервисы.

### E2E-тесты через pytest

Дополнительно к .NET-тестам — e2e-тесты на реальном запущенном сервисе:
- Располагаются в `/tests/mcp/` в корне репозитория
- Используют pytest (Python)
- Тестируют все MCP API: `list_apis`, `search_apis`, `api://{name}`, `resources/list`
- Запускаются против реально запущенного MCP-сервера
- Конфигурация URL через переменную окружения `MCP_URL`

В GitHub Actions e2e-тесты запускаются в отдельном workflow `.github/workflows/mcp-e2e.yml`:
- Поднимает docker-compose (wiki-api + mcp)
- Запускает pytest из `/tests/mcp/`
- Запускается при `push` и `pull_request`

Альтернатива — добавить e2e в существующий dotnet-ci.yml — отклонена: e2e требуют docker-compose и Python, что усложнит существующий workflow.

Фиксируется в **ADR-0011** (e2e-тесты через pytest).

### Code style Python

Python-код e2e-тестов следует минимально необходимому code style, фиксируемому в **ADR-0012** (Python code style).

## Риски / Компромиссы

- [Зависимость от wiki-api] → MCP-сервер недоступен если wiki-api недоступен. Митигация: возвращать понятную ошибку, не падать с необработанным исключением.
- [Версионирование MCP SDK] → SDK в активной разработке, API может меняться. Митигация: фиксировать версию пакета.
- [E2E-тесты требуют docker-compose в CI] → Усложняет CI. Митигация: отдельный workflow, docker доступен в GitHub Actions runners.

## План миграции

1. Создать проект `apps/mcp/`
2. Реализовать tools, resources, HTTP-клиент
3. Написать .NET-тесты (unit, integration, API)
4. Написать e2e-тесты в `/tests/mcp/`
5. Добавить Dockerfile
6. Добавить сервис `mcp` в `infrastructure/docker-compose/docker-compose.yaml`
7. Добавить workflow `.github/workflows/mcp-e2e.yml`
8. Зафиксировать ADR-0010, ADR-0011, ADR-0012

## Открытые вопросы

- Нет
