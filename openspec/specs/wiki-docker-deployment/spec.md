## ADDED Requirements

### Requirement: Docker Compose конфигурация

Репозиторий SHALL содержать файл `infrastructure/docker-compose/docker-compose.yaml`, запускающий wiki-сервис и Swagger UI как два отдельных контейнера.

Docker Compose проект SHALL иметь фиксированное имя `otus-05-mcp` (поле `name`). Контейнеры SHALL иметь фиксированные имена: `wiki-api` и `wiki-swagger-ui` (поле `container_name`).

#### Scenario: Фиксированные имена контейнеров

- **WHEN** docker-compose запущен
- **THEN** контейнеры доступны по именам `wiki-api` и `wiki-swagger-ui` (например, `docker logs wiki-api`)

#### Scenario: Запуск сервиса

- **WHEN** пользователь выполняет `docker compose up` из директории `infrastructure/docker-compose/`
- **THEN** запускаются контейнеры wiki-api и swagger-ui, оба доступны на хостовой машине

### Requirement: Доступность на хостовой машине

После запуска docker-compose SHALL быть доступны:

- Wiki REST API на порту `5000`
- Swagger UI на порту `5100`

#### Scenario: Доступ к Swagger UI

- **WHEN** docker-compose запущен
- **THEN** пользователь открывает `http://localhost:5100` и видит интерфейс Swagger UI с каталогом контрактов

#### Scenario: Доступ к REST API

- **WHEN** docker-compose запущен
- **THEN** запрос `GET http://localhost:5000/api/contracts` возвращает список контрактов

### Requirement: Монтирование папки контрактов

Docker Compose SHALL монтировать папку `apps/wiki/content/` с хостовой машины в контейнер wiki-api.

#### Scenario: Монтирование volume

- **WHEN** docker-compose запускает wiki-api контейнер
- **THEN** папка `../../apps/wiki/content` (относительно `infrastructure/docker-compose/`) смонтирована в контейнер

### Requirement: Сборка образа wiki-api

Docker Compose SHALL собирать образ wiki-api из `Dockerfile`, расположенного в `apps/wiki/`.

#### Scenario: Сборка при первом запуске

- **WHEN** пользователь выполняет `docker compose up --build`
- **THEN** образ wiki-api собирается из `apps/wiki/Dockerfile`
