## MODIFIED Requirements

### Requirement: Docker Compose конфигурация

Репозиторий SHALL содержать файл `infrastructure/docker-compose/docker-compose.yaml`, запускающий wiki-сервис, Swagger UI и MCP-сервер как три отдельных контейнера.

Docker Compose проект SHALL иметь фиксированное имя `otus-05-mcp` (поле `name`). Контейнеры SHALL иметь фиксированные имена: `wiki-api`, `wiki-swagger-ui` и `mcp` (поле `container_name`).

#### Scenario: Фиксированные имена контейнеров

- **WHEN** docker-compose запущен
- **THEN** контейнеры доступны по именам `wiki-api`, `wiki-swagger-ui` и `mcp` (например, `docker logs mcp`)

#### Scenario: Запуск сервиса

- **WHEN** пользователь выполняет `docker compose up` из директории `infrastructure/docker-compose/`
- **THEN** запускаются контейнеры wiki-api, swagger-ui и mcp, все доступны на хостовой машине

## ADDED Requirements

### Requirement: Доступность MCP-сервера

После запуска docker-compose MCP-сервер SHALL быть доступен на порту `5200`.

#### Scenario: Доступ к MCP-серверу

- **WHEN** docker-compose запущен
- **THEN** MCP-эндпоинт доступен по адресу `http://localhost:5200`

### Requirement: Сборка образа mcp

Docker Compose SHALL собирать образ mcp из `Dockerfile`, расположенного в `apps/mcp/`.

#### Scenario: Сборка при первом запуске

- **WHEN** пользователь выполняет `docker compose up --build`
- **THEN** образ mcp собирается из `apps/mcp/Dockerfile`
