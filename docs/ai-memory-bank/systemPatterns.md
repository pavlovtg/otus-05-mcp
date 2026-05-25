# System Patterns

## Архитектурные паттерны

- MCP-сервер: stdio или HTTP транспорт (решение предстоит)
- OpenSpec workflow: `spec-driven-with-adr` — proposal → specs → design → adr → tasks
- Contract-first: swagger-контракт создаётся в `docs/contracts/<service>/` до реализации (ADR-0005)
- Wiki REST API: динамическое чтение файлов из `CONTRACTS_PATH` при каждом запросе

## Правила работы с ADR

- ADR хранятся в `docs/adr/<domain>/`, кросс-доменные — в `docs/adr/general/`
- Индекс: `docs/adr/README.md` (одна строка на ADR)
- ADR иммутабельны; пересмотр — новый ADR с `supersedes`

## Структура .NET проектов (ADR-0007)

- `apps/<app>/src/<ProjectName>/` — исходный код
- `apps/<app>/tests/<ProjectName>.Tests/` — тесты
- `apps/<app>/<app>.slnx` — solution файл
- `docs/contracts/<service>/` — swagger-контракты сервисов

## Стратегия тестирования .NET (ADR-0008)

- Один тестовый проект `<ProjectName>.Tests` с тремя типами тестов
- Unit-тесты: бизнес-логика изолированно, без внешних зависимостей
- Integration-тесты: реальные зависимости (файловая система, БД)
- API-тесты: полный HTTP-стек через `WebApplicationFactory<TProgram>`
- Минимальный порог coverage: 80% line coverage

## GitHub Actions CI (ADR-0009)

- Workflow `.github/workflows/dotnet-ci.yml` при каждом push и pull_request
- Шаги: `dotnet restore` → `dotnet build` → `dotnet test` → проверка coverage ≥ 80%
- Branch protection rules блокируют слияние PR при падении CI

## Правила работы с кодом

- Язык: C# (.NET)
- Артефакты сборки игнорируются: `bin/`, `obj/`, `.vs/`

## Code style C# (ADR-0003)

- Отступы: табы
- Именование: `PascalCase` для классов/методов/свойств, `camelCase` для переменных, `_camelCase` для приватных полей, `IPascalCase` для интерфейсов
- Один публичный тип на файл; имя файла = имя типа

## Code style Markdown (ADR-0004)

- MD022: заголовки окружены пустыми строками
- MD047: файл заканчивается одним `\n`
- MD012: не более одной пустой строки подряд
- MD032: списки окружены пустыми строками
- MD034: URL в угловых скобках `<url>`
- MD009: нет trailing spaces
