# E2E-тесты MCP-сервера

Тесты проверяют MCP-сервер на реально запущенном сервисе.

## Требования

- Python 3.12+
- Запущенный MCP-сервер (через docker-compose или локально)

## Запуск через docker-compose

```bash
cd infrastructure/docker-compose
docker compose up -d
```

## Установка зависимостей

```bash
cd tests/mcp
pip install -r requirements.txt
```

## Запуск тестов

```bash
cd tests/mcp
MCP_URL=http://localhost:5200 pytest -v
```

По умолчанию `MCP_URL=http://localhost:5200`.

## Структура

- `conftest.py` — фикстуры и вспомогательные функции
- `test_mcp_tools.py` — тесты tools: `list_apis`, `search_apis`
- `test_mcp_resources.py` — тесты resources: `resources/list`, `api://{name}`
