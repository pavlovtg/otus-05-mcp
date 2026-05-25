# 0010. Стек MCP-сервера

- Статус: принят
- Дата: 2026-05-25

## Контекст

- Нужен MCP-сервер для предоставления AI-агентам доступа к каталогу API через wiki-api
- Репозиторий использует C# / .NET 10 (ADR-0003, ADR-0006)
- MCP поддерживает два транспорта: stdio и Streamable HTTP

## Решение

- Язык/платформа: C# / .NET 10, ASP.NET Core
- MCP SDK: официальный пакет `ModelContextProtocol` + `ModelContextProtocol.AspNetCore` (Microsoft, v1.3.0)
- Транспорт: Streamable HTTP (`WithHttpTransport()`, `MapMcp()`)
- Интеграция с wiki-api: типизированный `HttpClient` через `IWikiApiClient`
- Конфигурация: `WIKI_API_URL` через переменную окружения
- Порт: 5200

## Последствия

- Stdio отклонён: требует запуска как дочернего процесса IDE, несовместим с Docker
- HTTP позволяет подключаться нескольким клиентам и деплоиться через Docker
- SDK в активной разработке — версия пакета фиксируется явно
- Структура проекта следует ADR-0007 (`apps/mcp/src/`, `apps/mcp/tests/`)
