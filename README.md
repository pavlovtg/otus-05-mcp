# otus-05-mcp

Простой MCP-сервер

## Wiki — каталог swagger-контрактов

Wiki-сервис предоставляет REST API и web-интерфейс для просмотра swagger-контрактов из папки `apps/wiki/content/`.

### Запуск через Docker Compose

**Требования:** Docker Desktop или Docker Engine с Docker Compose.

```bash
cd infrastructure/docker-compose
docker compose up --build
```

После запуска доступны:

- **Swagger UI** — <http://localhost:5100> — web-интерфейс для просмотра контрактов
- **Wiki REST API** — <http://localhost:5000> — программный доступ к каталогу

Управление контейнерами:

```bash
# Остановить
docker compose down

# Просмотр логов
docker logs wiki-api
docker logs wiki-swagger-ui
```

### Wiki REST API

| Метод | Путь | Описание |
|-------|------|----------|
| `GET` | `/api/contracts` | Список всех контрактов |
| `GET` | `/api/contracts/{name}` | Содержимое контракта по имени файла |
| `GET` | `/api/contracts/search?q=` | Поиск по имени, title, description |

Пример:

```bash
# Список контрактов
curl http://localhost:5000/api/contracts

# Получить конкретный контракт
curl http://localhost:5000/api/contracts/EventStream.Contracts.yaml

# Поиск
curl "http://localhost:5000/api/contracts/search?q=assets"
```

### Добавление контрактов

Положите `.yaml`-файл в папку `apps/wiki/content/` — он сразу появится в REST API.

Для обновления списка в Swagger UI перезапустите контейнер `swagger-ui`:

```bash
docker compose restart swagger-ui
```
