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

## MCP-сервер

MCP-сервер предоставляет AI-агентам (Cline, Claude Desktop и др.) доступ к каталогу API через Model Context Protocol.
Транспорт: Streamable HTTP, порт 5200.

### Запуск

MCP-сервер запускается вместе с остальными сервисами через Docker Compose:

```bash
cd infrastructure/docker-compose
docker compose up --build
```

После запуска MCP-сервер доступен по адресу <http://localhost:5200>.

Просмотр логов:

```bash
docker logs mcp
```

### Подключение Cline

> **Важно:** Cline VS Code расширение не поддерживает project-level MCP конфиг —
> конфиг **не хранится в репозитории** и настраивается вручную на каждой машине.

**Вариант 1 — через панель Cline (рекомендуется):**

1. Убедитесь что MCP-сервер запущен (`docker compose up`).
2. В панели Cline нажмите иконку **MCP Servers** (стопка серверов в тулбаре).
3. Откройте вкладку **Remote Servers**.
4. Введите: имя `otus-mcp`, URL `http://localhost:5200/`, тип `Streamable HTTP`.
5. Нажмите **Add Server**.

**Вариант 2 — через JSON:**

1. В панели Cline → MCP Servers → Configure → **Configure MCP Servers**.
2. Добавьте в открывшийся файл:

```json
{
  "mcpServers": {
    "otus-mcp": {
      "type": "streamableHttp",
      "url": "http://localhost:5200/",
      "disabled": false,
      "autoApprove": []
    }
  }
}
```

Подробнее: <https://docs.cline.bot/mcp/mcp-overview>

### Доступные инструменты (Tools)

| Инструмент | Описание |
|------------|----------|
| `list_apis` | Возвращает список всех доступных API внутренних сервисов |
| `search_apis` | Поиск API по имени или описанию (параметр: `query`) |

### Доступные ресурсы (Resources)

| URI | Описание |
|-----|----------|
| `api://{name}` | Полное содержимое Swagger YAML-файла для указанного API |
