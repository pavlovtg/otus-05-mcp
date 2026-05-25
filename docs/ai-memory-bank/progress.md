# Progress

## Сделано

- Создана структура репозитория (openspec, docs/adr)
- ADR 0001: структура хранения ADR принята
- OpenSpec change `init-repository-rules` создан, заархивирован
- Правила Cline, memory bank, gitignore, .clineignore созданы
- OpenSpec change `init-repository-adrs` создан и реализован
- ADR 0002: структура директорий репозитория принята
- ADR 0003: code style для C# принят
- ADR 0004: code style для Markdown принят
- Шаблоны OpenSpec обновлены под Markdown code style (ADR-0004)
- OpenSpec change `wiki-api-service` реализован
- ADR 0005: contract-first подход для сервисов репозитория принят
- ADR 0006: стек wiki-сервиса (ASP.NET Core + Swagger UI) принят
- ADR 0007: структура .NET проектов в репозитории принята
- ADR 0008: стратегия тестирования .NET сервисов принята
- ADR 0009: GitHub Actions CI для .NET сервисов принят
- Wiki REST API создан: `apps/wiki/src/Wiki.Api/` (ASP.NET Core, .NET 10)
- Тесты созданы: 28 тестов, coverage 93.87% (unit + integration + API)
- Docker: `apps/wiki/Dockerfile` + `infrastructure/docker-compose/docker-compose.yaml`
- GitHub Actions CI: `.github/workflows/dotnet-ci.yml`
- Swagger-контракт wiki API: `docs/contracts/wiki/wiki-api.yaml`

## В процессе

- Нет

## Осталось

- Разработка MCP-сервера на .NET
- Определение инструментов и ресурсов MCP
- Настройка branch protection rules в GitHub (ручная)

## Известные проблемы

- Нет
