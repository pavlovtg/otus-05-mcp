# Active Context

## Текущий фокус

- Change `wiki-api-service` реализован
- Wiki REST API работает: 28 тестов, coverage 93.87%
- Следующий шаг: разработка MCP-сервера на .NET

## Активные задачи

- Архивация change `wiki-api-service`
- Настройка branch protection rules в GitHub (ручная)

## Последние решения

- Структура директорий: `/apps`, `/infrastructure`, `/docs`, `/homework` (ADR-0002)
- Code style C#: табы, PascalCase/camelCase (ADR-0003)
- Code style Markdown: MD009, MD012, MD022, MD032, MD034, MD047 (ADR-0004)
- Contract-first подход: контракт в `docs/contracts/<service>/` до реализации (ADR-0005)
- Стек wiki-сервиса: ASP.NET Core + Swagger UI в Docker Compose (ADR-0006)
- Структура .NET проектов: `apps/<app>/src/`, `apps/<app>/tests/`, `<app>.slnx` (ADR-0007)
- Стратегия тестирования: unit + integration + API-тесты, coverage ≥ 80% (ADR-0008)
- GitHub Actions CI: сборка + тесты + coverage при push/PR (ADR-0009)
