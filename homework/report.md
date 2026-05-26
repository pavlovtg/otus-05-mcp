# Отчёт по домашнему заданию: Простой MCP-сервер

## Назначение MCP-сервера

В организации существует набор внутренних сервисов, API которых описаны в формате **Swagger/OpenAPI**. Swagger-файлы хранятся в сервисе **Wiki API** (`apps/wiki/`) — он предоставляет REST API для доступа к каталогу контрактов: получение списка всех API, поиск по имени/описанию, чтение содержимого конкретного контракта. Swagger-файлы сервис берет из папки на диске (`apps/wiki/content/`).

**MCP-сервер** (`apps/mcp/`) — это мост между AI-агентами (Cline, Claude Desktop и др.) и Wiki API. Он предоставляет инструменты по протоколу Model Context Protocol, чтобы агент мог прямо из IDE находить нужные API-контракты, читать их содержимое и использовать эту информацию для генерации кода, написания документации или ответов на вопросы о внутренних сервисах организации.

---

## 1. Принципы MCP

### Как IDE/агент подключается к MCP-серверу

Model Context Protocol (MCP) — стандартный протокол для интеграции AI-агентов с внешними инструментами. Агент (например, Cline в VS Code) подключается к MCP-серверу по HTTP, используя транспорт **Streamable HTTP**. При подключении клиент отправляет запрос `initialize`, получает список capabilities (tools и resources), после чего может вызывать инструменты через `tools/call` и читать ресурсы через `resources/read`. В данном проекте сервер работает в **stateless-режиме**: каждый запрос обрабатывается независимо, без хранения сессий — это упрощает деплой через Docker.

### Что такое «tool» в нашем сервере

**Tool** — это функция, которую агент может вызвать по имени с параметрами и получить структурированный JSON-ответ. В нашем сервере реализованы два инструмента: `list_apis` (возвращает список всех API-контрактов) и `search_apis` (фильтрует список по строке запроса). Помимо tools, сервер предоставляет **resource** `api://{name}` — именованный источник данных, возвращающий полное содержимое Swagger YAML-файла. Разница: tool — это действие с параметрами, resource — это адресуемый контент.

---

## 2. Запуск MCP-сервера

### Требования

- Docker Desktop или Docker Engine с Docker Compose

### Запуск

```bash
cd infrastructure/docker-compose
docker compose up --build
```

После запуска доступны:

- **MCP-сервер** — `http://localhost:5200` (Streamable HTTP транспорт)
- **Wiki REST API** — `http://localhost:5000`
- **Swagger UI** — `http://localhost:5100`

### Просмотр логов

```bash
docker logs mcp
docker logs wiki-api
```

### Остановка

```bash
docker compose down
```

---

## 3. Реализованные инструменты и ресурсы

### Tool: `list_apis`

**Описание:** Возвращает список всех доступных API внутренних сервисов.

**Параметры:** нет.

**Реализация:** [`apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs:L21–L38`](../apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs)

**Формат результата:** массив объектов `ContractInfo`:

```json
[
  {
    "name": "Analytics.DataExporter.V1.yaml",
    "title": "AnalyticsDataExporter",
    "version": "1.0.0",
    "description": "API сервиса экспорта данных"
  }
]
```

**Контракт результата:** [`apps/mcp/src/Mcp.Api/Models/ContractInfo.cs:L1–L9`](../apps/mcp/src/Mcp.Api/Models/ContractInfo.cs)

**Лог успешного вызова:**

```
2026-05-26 22:27:05 INF [MCP] Tool invoked Tool=list_apis Params={} Status=success Count=22
```

---

### Tool: `search_apis`

**Описание:** Поиск API по имени или описанию.

**Параметры:**

| Параметр | Тип | Обязательный | Описание |
|----------|-----|--------------|----------|
| `query` | string | да | Строка поиска |

**Реализация:** [`apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs:L40–L63`](../apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs)

**Формат результата:** массив объектов `ContractInfo` (тот же формат, что и `list_apis`).

**Лог успешного вызова:**

```
2026-05-26 22:27:05 INF [MCP] Tool invoked Tool=search_apis Params={query=Vulnerabilities} Status=success Count=2
```

---

### Resource: `api://{name}`

**Описание:** Возвращает полное содержимое Swagger YAML-файла для указанного API.

**Параметры URI:**

| Параметр | Описание |
|----------|----------|
| `name` | Имя файла контракта (например, `Analytics.DataExporter.V1.yaml`) |

**Реализация:** [`apps/mcp/src/Mcp.Api/Resources/ApiContractResources.cs:L20–L37`](../apps/mcp/src/Mcp.Api/Resources/ApiContractResources.cs)

**Формат результата:** строка с содержимым YAML-файла.

**Лог успешного вызова:**

```
2026-05-26 22:28:07 INF [MCP] Resource requested Resource=api://TimeZones.Contracts.yaml Params={name=TimeZones.Contracts.yaml} Status=success
```

---

## 4. Регистрация сервера и инструментов

**Файл:** [`apps/mcp/src/Mcp.Api/Program.cs:L1–L34`](../apps/mcp/src/Mcp.Api/Program.cs)

```csharp
builder.Services
    .AddMcpServer()
    .WithHttpTransport(options => options.Stateless = true)
    .WithTools<ApiDiscoveryTools>()
    .WithResources<ApiContractResources>();

app.MapMcp();
```

- `WithHttpTransport(Stateless = true)` — Streamable HTTP без хранения сессий
- `WithTools<ApiDiscoveryTools>()` — регистрация tools `list_apis` и `search_apis`
- `WithResources<ApiContractResources>()` — регистрация resource `api://{name}`
- `MapMcp()` — маппинг MCP-эндпоинта на `/`

---

## 5. Интеграция с агентом в IDE (Cline VS Code)

> **Важно:** Cline VS Code расширение не поддерживает project-level MCP конфиг — конфиг **не хранится в репозитории** и настраивается вручную на каждой машине.

### Вариант 1 — через панель Cline (рекомендуется)

1. Убедитесь что MCP-сервер запущен (`docker compose up`).
2. В панели Cline нажмите иконку **MCP Servers** (стопка серверов в тулбаре).
3. Откройте вкладку **Remote Servers**.
4. Введите: имя `otus-mcp`, URL `http://localhost:5200/`, тип `Streamable HTTP`.
5. Нажмите **Add Server**.

### Вариант 2 — через JSON-конфиг

1. В панели Cline → MCP Servers → Configure → **Configure MCP Servers**.
2. Добавьте в открывшийся файл `cline_mcp_settings.json`:

```json
{
  "mcpServers": {
    "otus-mcp": {
      "type": "streamableHttp",
      "url": "http://localhost:5200/",
      "disabled": false,
      "autoApprove": []
    }
  }
}
```

---

## 6. Проверочные запросы

Все запросы выполнены агентом Cline в IDE. Агент самостоятельно определял нужный MCP-tool и вызывал его.

---

### Запрос 1: «Какие API есть для доступа к Vulnerabilities»

**Ожидаемый tool:** `search_apis(query="Vulnerabilities")`

**Фактический вызов:** ✅ `search_apis` вызван

**Результат:**

```json
[
  {"name": "Vulnerabilities.Contracts.yaml", "title": "Vulnerabilities", "version": "1.0.0", "description": "API запросов информации по уязвимостям"},
  {"name": "AssetVulnerabilities.Contracts.yaml", "title": "AssetVulnerabilities", "version": "1.0.0", "description": "Contract to modify vulnerabilities instances"}
]
```

**Лог MCP-сервера:**

```
2026-05-26 22:27:05 INF [MCP] Tool invoked Tool=search_apis Params={query=Vulnerabilities} Status=success Count=2
```

**Лог wiki-api:**

```
2026-05-26 22:27:05 INF [WikiAPI] Request handled Endpoint=GET /api/contracts/search Params={q=Vulnerabilities} Status=200 Count=2
```

---

### Запрос 2: «Напиши .NET клиент доступа к Dictionaries 1.0.0 и помести в /homework/Dictionaries.Client»

**Ожидаемые tools:** `search_apis(query="Dictionaries")` + resource `api://Dictionaries.Contracts.yaml`

**Фактический вызов:** ✅ `search_apis` + resource вызваны, клиент создан в [`homework/Dictionaries.Client/DictionariesClient.cs`](Dictionaries.Client/DictionariesClient.cs)

**Лог MCP-сервера:**

```
2026-05-26 22:27:16 INF [MCP] Tool invoked Tool=search_apis Params={query=Dictionaries} Status=success Count=1
2026-05-26 22:27:20 INF [MCP] Resource requested Resource=api://Dictionaries.Contracts.yaml Params={name=Dictionaries.Contracts.yaml} Status=success
```

**Лог wiki-api:**

```
2026-05-26 22:27:16 INF [WikiAPI] Request handled Endpoint=GET /api/contracts/search Params={q=Dictionaries} Status=200 Count=1
2026-05-26 22:27:20 INF [WikiAPI] Request handled Endpoint=GET /api/contracts/{name} Params={name=Dictionaries.Contracts.yaml} Status=200
```

---

### Запрос 3: «Сколько версий контрактов есть для Triggers»

**Ожидаемый tool:** `search_apis(query="Triggers")`

**Фактический вызов:** ✅ `search_apis` вызван

**Результат:** 5 контрактов:

- `MetaTriggers.Contracts.v2.yaml` (MetaTriggersV2, v2.0)
- `Triggers.Contracts.v2.yaml` (Triggersv2, v2.0)
- `Triggers.Contracts.yaml` (Triggers, v1.0) — устаревший
- `MetaTriggers.Contracts.yaml` (MetaTriggers, v1.0) — устаревший
- `DebugTriggers.Contracts.yaml` (DebugTriggers, v1.0)

Итого: 2 версии для Triggers (v1.0 и v2.0), 2 версии для MetaTriggers (v1.0 и v2.0).

**Лог MCP-сервера:**

```
2026-05-26 22:27:55 INF [MCP] Tool invoked Tool=search_apis Params={query=Triggers} Status=success Count=5
```

**Лог wiki-api:**

```
2026-05-26 22:27:55 INF [WikiAPI] Request handled Endpoint=GET /api/contracts/search Params={q=Triggers} Status=200 Count=5
```

---

### Запрос 4: «Дай описание API для Abracadabra»

**Ожидаемый tool:** `search_apis(query="Abracadabra")`

**Фактический вызов:** ✅ `search_apis` вызван

**Результат:** пустой массив `[]` — API с таким именем не найдено.

**Лог MCP-сервера:**

```
2026-05-26 22:27:59 INF [MCP] Tool invoked Tool=search_apis Params={query=Abracadabra} Status=success Count=0
```

**Лог wiki-api:**

```
2026-05-26 22:27:59 INF [WikiAPI] Request handled Endpoint=GET /api/contracts/search Params={q=Abracadabra} Status=200 Count=0
```

---

### Запрос 5: «Покажи методы API TimeZones»

**Ожидаемые tools:** `search_apis(query="TimeZones")` + resource `api://TimeZones.Contracts.yaml`

**Фактический вызов:** ✅ `search_apis` + resource вызваны

**Результат:** API TimeZones v1.0 содержит один метод:

| Метод | Путь | Описание |
|-------|------|----------|
| `GET` | `/api/triggers/v1/time_zones` | Возвращает список таймзон, доступных на сервере |

**Лог MCP-сервера:**

```
2026-05-26 22:28:03 INF [MCP] Tool invoked Tool=search_apis Params={query=TimeZones} Status=success Count=1
2026-05-26 22:28:07 INF [MCP] Resource requested Resource=api://TimeZones.Contracts.yaml Params={name=TimeZones.Contracts.yaml} Status=success
```

**Лог wiki-api:**

```
2026-05-26 22:28:03 INF [WikiAPI] Request handled Endpoint=GET /api/contracts/search Params={q=TimeZones} Status=200 Count=1
2026-05-26 22:28:07 INF [WikiAPI] Request handled Endpoint=GET /api/contracts/{name} Params={name=TimeZones.Contracts.yaml} Status=200
```

---

## 7. Логирование

### Реализация логирования

Логирование реализовано через Serilog. Конфигурация: [`apps/mcp/src/Mcp.Api/Program.cs:L8–L11`](../apps/mcp/src/Mcp.Api/Program.cs)

```csharp
builder.Host.UseSerilog((ctx, cfg) => cfg
    .MinimumLevel.Information()
    .Enrich.WithProperty("service", "MCP")
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3} [{service}] {Message:lj} {Properties:j}{NewLine}{Exception}"));
```

### Логирование tools

Файл: [`apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs:L28–L29`](../apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs) (success) и `L34–L35` (error)

**Успех:**

```csharp
_logger.LogInformation("Tool invoked Tool={Tool} Params={{}} Status=success Count={Count}",
    "list_apis", result.Count);
```

**Ошибка:**

```csharp
_logger.LogError(ex, "Tool failed Tool={Tool} Params={{}} Status=error Error={Error}",
    "list_apis", ex.GetType().Name);
```

### Логирование resources

Файл: [`apps/mcp/src/Mcp.Api/Resources/ApiContractResources.cs:L27–L28`](../apps/mcp/src/Mcp.Api/Resources/ApiContractResources.cs) (success) и `L33–L34` (error)

**Успех:**

```csharp
_logger.LogInformation("Resource requested Resource={Resource} Params={{name={Name}}} Status=success",
    $"api://{name}", name);
```

### Пример реального вывода логов (docker logs mcp)

```
2026-05-26 22:27:05 INF [MCP] Tool invoked Tool=search_apis Params={query=Vulnerabilities} Status=success Count=2 {"SourceContext": "Mcp.Api.Tools.ApiDiscoveryTools", ...}
2026-05-26 22:27:16 INF [MCP] Tool invoked Tool=search_apis Params={query=Dictionaries} Status=success Count=1 {"SourceContext": "Mcp.Api.Tools.ApiDiscoveryTools", ...}
2026-05-26 22:27:20 INF [MCP] Resource requested Resource=api://Dictionaries.Contracts.yaml Params={name=Dictionaries.Contracts.yaml} Status=success {"SourceContext": "Mcp.Api.Resources.ApiContractResources", ...}
2026-05-26 22:27:55 INF [MCP] Tool invoked Tool=search_apis Params={query=Triggers} Status=success Count=5 {"SourceContext": "Mcp.Api.Tools.ApiDiscoveryTools", ...}
2026-05-26 22:27:59 INF [MCP] Tool invoked Tool=search_apis Params={query=Abracadabra} Status=success Count=0 {"SourceContext": "Mcp.Api.Tools.ApiDiscoveryTools", ...}
2026-05-26 22:28:03 INF [MCP] Tool invoked Tool=search_apis Params={query=TimeZones} Status=success Count=1 {"SourceContext": "Mcp.Api.Tools.ApiDiscoveryTools", ...}
2026-05-26 22:28:07 INF [MCP] Resource requested Resource=api://TimeZones.Contracts.yaml Params={name=TimeZones.Contracts.yaml} Status=success {"SourceContext": "Mcp.Api.Resources.ApiContractResources", ...}
```

---

## 8. Безопасность

- Секреты не коммитятся: `WIKI_API_URL` передаётся через переменную окружения Docker Compose
- Файл `.env.example` не требуется — конфигурация задаётся через `environment:` в `docker-compose.yaml`
- MCP-сервер не выполняет команды и не читает произвольные файлы — только обращается к wiki-api по HTTP
- Логирование не включает секретные данные (параметр `query` не является секретом)

---

## 9. Ссылки на код (обязательные)

### 1. Реализация MCP-сервера — регистрация и запуск

**Файл:** [`apps/mcp/src/Mcp.Api/Program.cs:L1–L34`](../apps/mcp/src/Mcp.Api/Program.cs)

- `L6` — создание WebApplication builder
- `L8–L11` — конфигурация Serilog
- `L22–L26` — регистрация MCP-сервера, транспорта, tools и resources
- `L30` — маппинг MCP-эндпоинта

### 2. Реализация инструментов

**Tool `list_apis`:**

- Реализация: [`apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs:L21–L38`](../apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs)
- Логирование: `L28–L29` (success), `L34–L35` (error)
- Пример вывода: `Tool invoked Tool=list_apis Params={} Status=success Count=22`

**Tool `search_apis`:**

- Реализация: [`apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs:L40–L63`](../apps/mcp/src/Mcp.Api/Tools/ApiDiscoveryTools.cs)
- Логирование: `L53–L54` (success), `L59–L60` (error)
- Пример вывода: `Tool invoked Tool=search_apis Params={query=Triggers} Status=success Count=5`

**Resource `api://{name}`:**

- Реализация: [`apps/mcp/src/Mcp.Api/Resources/ApiContractResources.cs:L20–L37`](../apps/mcp/src/Mcp.Api/Resources/ApiContractResources.cs)
- Логирование: `L27–L28` (success), `L33–L34` (error)
- Пример вывода: `Resource requested Resource=api://TimeZones.Contracts.yaml Params={name=TimeZones.Contracts.yaml} Status=success`

### 3. Агент корректно вызывает нужный tool

**Пример:** запрос «Сколько версий контрактов есть для Triggers»

- Ожидаемый tool: `search_apis(query="Triggers")`
- Фактическое подтверждение (лог): `Tool invoked Tool=search_apis Params={query=Triggers} Status=success Count=5`
- Лог хранится в stdout контейнера `mcp` (см. `docker logs mcp`)

### 4. Контракт результатов tool

**Модель `ContractInfo`:** [`apps/mcp/src/Mcp.Api/Models/ContractInfo.cs:L1–L9`](../apps/mcp/src/Mcp.Api/Models/ContractInfo.cs)

```csharp
public class ContractInfo
{
    public string Name { get; set; }        // имя файла контракта
    public string Title { get; set; }       // заголовок из OpenAPI info.title
    public string Version { get; set; }     // версия из OpenAPI info.version
    public string Description { get; set; } // описание из OpenAPI info.description
}
```

Tools `list_apis` и `search_apis` возвращают `IReadOnlyList<ContractInfo>`, сериализованный в JSON.
Resource `api://{name}` возвращает строку с содержимым YAML-файла.
