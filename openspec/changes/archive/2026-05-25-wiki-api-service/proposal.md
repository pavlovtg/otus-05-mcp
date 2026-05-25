## Зачем

В репозитории накапливаются swagger-контракты внутренних сервисов без единой точки доступа. Нужен wiki-сервис, который предоставляет удобный UI для просмотра контрактов людьми и REST API для программных потребителей (MCP-сервер и др.). Одновременно фиксируются архитектурные решения по структуре .NET проектов, расположению контрактов сервисов, стратегии тестирования и CI.

## Что меняется

- Создать ASP.NET Core сервис `apps/wiki/` — REST API для каталога swagger-контрактов с поддержкой CORS
- Создать swagger-контракт для wiki REST API в `docs/contracts/wiki/` (contract-first подход)
- Интегрировать Swagger UI (`swaggerapi/swagger-ui`) как web-интерфейс для просмотра контрактов
- Создать `infrastructure/docker-compose/docker-compose.yaml` для развёртывания wiki-сервиса с монтированием папки контрактов, фиксированными именами контейнеров (`wiki-api`, `wiki-swagger-ui`) и именем проекта `otus-05-mcp`
- Создать GitHub Actions workflow для сборки, тестов и проверки code coverage
- Зафиксировать структуру .NET проектов в ADR: `apps/<app>/src`, `apps/<app>/tests`, `apps/<app>/<app>.slnx`
- Зафиксировать расположение контрактов сервисов репозитория в ADR: `docs/contracts/<service>/*.yaml`
- Зафиксировать стратегию тестирования .NET сервисов в ADR: unit, integration, API-тесты, coverage ≥ 80%
- Зафиксировать CI-подход в ADR: сборка + тесты + coverage при push/PR, блокировка PR при падении

## Возможности

### Новые возможности

- `wiki-rest-api`: REST API для поиска и получения swagger-контрактов
- `wiki-web-ui`: Web-интерфейс на базе Swagger UI для просмотра контрактов (read-only)
- `wiki-contract-source`: Источник контрактов — папка со swagger-файлами, монтируется в контейнер
- `wiki-docker-deployment`: Docker Compose конфигурация для развёртывания сервиса
- `dotnet-project-structure`: Стандартная структура .NET проектов в репозитории
- `dotnet-testing-strategy`: Стратегия тестирования .NET сервисов (unit, integration, API-тесты, coverage ≥ 80%)
- `github-ci`: GitHub Actions CI — сборка, тесты, coverage при push/PR с блокировкой PR

### Изменённые возможности

_(нет)_

## Влияние

- `apps/wiki/` — новый ASP.NET Core проект (структура: `src/`, `tests/`, `wiki.slnx`)
- `apps/wiki/content/` — папка с swagger-файлами (данные wiki-сервиса, монтируется через volume)
- `docs/contracts/wiki/` — swagger-контракт wiki REST API (contract-first)
- `infrastructure/docker-compose/docker-compose.yaml` — конфигурация развёртывания
- `.github/workflows/` — GitHub Actions workflow для CI
- `docs/adr/` — новые ADR: contract-first подход, стек wiki-сервиса, структура .NET проектов, расположение контрактов, стратегия тестирования, CI-подход
- `README.md` — добавлена инструкция по запуску через Docker Compose
- `.clineignore` — добавлена `apps/wiki/content/`
