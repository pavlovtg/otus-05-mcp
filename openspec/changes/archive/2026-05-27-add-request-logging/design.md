## Контекст

Оба сервиса (`mcp` и `wiki-api`) работают в Docker Compose и пишут вывод в stdout. После прогона E2E-тестов логи собираются командой `docker compose logs` и используются для формирования отчёта. Сейчас ни один из сервисов не логирует детали обработки запросов.

## Цели / Не-цели

**Цели:**

- Добавить логирование вызовов инструментов и ресурсов в MCP-сервере
- Добавить логирование входящих HTTP-запросов в wiki-api
- Зафиксировать единый формат лог-записей для обоих сервисов
- Обеспечить возможность сбора логов через `docker compose logs`

**Не-цели:**

- Централизованный сбор логов (ELK, Loki, Grafana)
- Трассировка запросов между сервисами (distributed tracing)
- Метрики и алерты

## Решения

### Формат лог-записи (единый для всех сервисов)

```
{Timestamp:YYYY-MM-DD HH:mm:ss} {LEVEL} [{service}] Message {Properties}
```

- `{service}` — идентификатор сервиса: `MCP` или `WikiAPI`
- `{LEVEL}` — уровень логирования: `INF`, `ERR` и т.д.
- `Message` — краткое описание события
- `{Properties}` — структурированные свойства в формате `Key=Value`

Примеры:

```
2026-05-27 12:05:01 INF [MCP] Tool invoked Tool=list_apis Params={} Status=success Count=22
2026-05-27 12:05:02 INF [MCP] Tool invoked Tool=search_apis Params={query="triggers"} Status=success Count=4
2026-05-27 12:05:03 ERR [MCP] Tool failed Tool=search_apis Params={query="..."} Status=error Error=WikiApiException
2026-05-27 12:05:04 INF [MCP] Resource requested Resource=api://{name} Params={name="Triggers.Contracts.yaml"} Status=success
2026-05-27 12:05:05 INF [WikiAPI] Request handled Endpoint=GET /api/contracts Params={} Status=200 Count=22
2026-05-27 12:05:06 INF [WikiAPI] Request handled Endpoint=GET /api/contracts/search Params={q="triggers"} Status=200 Count=4
2026-05-27 12:05:07 ERR [WikiAPI] Request failed Endpoint=GET /api/contracts/{name} Params={name="..."} Status=500 Error=Exception
```

Формат фиксируется в ADR-0013 и является обязательным для всех сервисов проекта.

### Механизм логирования: `ILogger<T>` через DI + OutputTemplate

Оба сервиса используют ASP.NET Core. Для достижения нужного формата настраиваем `ConsoleFormatterOptions` с кастомным `OutputTemplate` в `Program.cs`.

**Альтернативы:**
- Serilog — поддерживает произвольные шаблоны, но требует дополнительного пакета
- `Console.WriteLine` — не структурировано, не управляется уровнями логирования
- Встроенный `SimpleConsoleFormatter` — не поддерживает произвольный шаблон с `{service}`

**Решение:** используем Serilog с `outputTemplate` — минимальная зависимость, полный контроль над форматом.

### Место логирования в MCP-сервере: в методах Tools и Resources

Логируем непосредственно в `ApiDiscoveryTools` и `ApiContractResources` — там известны имя инструмента/ресурса и параметры.

**Альтернатива:** middleware/pipeline-перехватчик на уровне MCP SDK — SDK не предоставляет публичного хука, поэтому логируем в методах напрямую.

### Место логирования в wiki-api: в методах контроллера

Логируем в `ContractsController` — там известны параметры запроса и статус ответа.

**Альтернатива:** ASP.NET Core middleware — логирует все запросы автоматически, но не даёт доступа к бизнес-параметрам. Для данной задачи достаточно логирования в контроллере.

### Сбор логов после тестов

```bash
docker compose logs wiki-api > logs/wiki-api.log
docker compose logs mcp      > logs/mcp.log
```

## Риски / Компромиссы

- [Serilog — дополнительная зависимость] → Минимальный пакет `Serilog.AspNetCore`, широко используется в .NET-экосистеме
- [Логирование в методах контроллера дублирует часть информации из встроенного ASP.NET Core request logging] → Встроенный логгер не даёт доступа к бизнес-параметрам, поэтому дублирование приемлемо

## План миграции

- Изменения обратно совместимы: добавляется только логирование, поведение API не меняется
- Откат: удалить вызовы `_logger` из методов и убрать Serilog из `Program.cs`

## Открытые вопросы

- Нет
