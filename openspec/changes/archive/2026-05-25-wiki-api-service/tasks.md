## 1. Контракт wiki REST API (contract-first)

- [x] 1.1 Создать `docs/contracts/wiki/wiki-api.yaml` — swagger-контракт wiki REST API (эндпоинты: list, get, search)

## 2. Структура проекта

- [x] 2.1 Создать структуру `apps/wiki/src/Wiki.Api/` — ASP.NET Core проект
- [x] 2.2 Создать структуру `apps/wiki/tests/Wiki.Api.Tests/` — тестовый проект
- [x] 2.3 Создать `apps/wiki/wiki.slnx` — solution файл

## 3. Реализация wiki REST API

- [x] 3.1 Реализовать `GET /api/contracts` — список контрактов из `CONTRACTS_PATH`
- [x] 3.2 Реализовать `GET /api/contracts/{name}` — получение YAML-файла по имени
- [x] 3.3 Реализовать `GET /api/contracts/search?q=` — поиск по имени, title, description
- [x] 3.4 Добавить валидацию имени файла (защита от path traversal)
- [x] 3.5 Настроить `CONTRACTS_PATH` через переменную окружения (default: `./content`)

## 4. Тесты

- [x] 4.1 Написать unit-тесты бизнес-логики (поиск, парсинг YAML, валидация имён)
- [x] 4.2 Написать integration-тесты чтения файлов из временной директории
- [x] 4.3 Написать API-тесты через `WebApplicationFactory` для всех трёх эндпоинтов
- [x] 4.4 Проверить code coverage ≥ 80%

## 5. Docker

- [x] 5.1 Создать `apps/wiki/Dockerfile` для wiki-api
- [x] 5.2 Создать `infrastructure/docker-compose/docker-compose.yaml` — wiki-api (порт 5000) + swagger-ui (порт 5100)
- [x] 5.3 Настроить монтирование `apps/wiki/content/` в контейнер wiki-api
- [x] 5.4 Настроить `URLS` для Swagger UI — список эндпоинтов `GET /api/contracts/{name}`
- [ ] 5.5 Проверить запуск: `docker compose up --build`

## 6. GitHub Actions CI

- [x] 6.1 Создать `.github/workflows/dotnet-ci.yml` — сборка, тесты, coverage при push/PR
- [ ] 6.2 Настроить branch protection rules в GitHub (блокировка PR при падении CI)

## 7. Обновление memory bank

- [x] 7.1 Обновить `docs/ai-memory-bank/progress.md` — зафиксировать создание wiki-сервиса и ADR-0005..0009
- [x] 7.2 Обновить `docs/ai-memory-bank/activeContext.md` — обновить текущий фокус
- [x] 7.3 Обновить `docs/ai-memory-bank/techContext.md` — добавить .NET 10, Docker Compose, GitHub Actions
- [x] 7.4 Обновить `docs/ai-memory-bank/systemPatterns.md` — добавить паттерны тестирования и CI
