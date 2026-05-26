## Контекст

- MCP-сервер работает на ASP.NET Core, порт 5200, транспорт Streamable HTTP (ADR-0010)
- Cline VS Code расширение **не поддерживает** project-level MCP конфиг — `.cline/mcp.json` в репозитории не читается расширением
- MCP конфиг для VS Code расширения хранится только глобально: `~/Library/Application Support/Code/User/globalStorage/saoudrizwan.claude-dev/settings/cline_mcp_settings.json`
- Docker Compose поднимает wiki-api (5000) + mcp (5200) + swagger-ui (5100)
- `README.md` существует в корне репозитория — нужно дополнить разделом про MCP

## Цели / Не-цели

**Цели:**

- Дополнить `README.md` разделом про MCP-сервер: запуск и ручное подключение агента через глобальный конфиг

**Не-цели:**

- Хранение MCP конфига в репозитории (невозможно для VS Code расширения)
- Изменение реализации MCP-сервера
- Добавление логирования (отдельный change)

## Решения

### Конфигурация Cline MCP (только глобальная)

Cline VS Code расширение читает MCP конфиг только из глобального файла:

```
~/Library/Application Support/Code/User/globalStorage/saoudrizwan.claude-dev/settings/cline_mcp_settings.json
```

Способы добавить сервер:

**Вариант 1 — через панель Cline (рекомендуется):**

1. Нажать иконку MCP Servers в тулбаре Cline
2. Открыть вкладку Remote Servers
3. Ввести имя, URL `http://localhost:5200/`, тип `Streamable HTTP`
4. Нажать Add Server

**Вариант 2 — через JSON:**

Открыть Configure MCP Servers и добавить:

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

### Дополнение README.md

В `README.md` добавляется раздел `## MCP-сервер` с:

1. Описанием назначения MCP-сервера
2. Инструкцией по запуску через Docker Compose
3. Инструкцией по подключению Cline (оба варианта)
4. Явным указанием что конфиг не хранится в репозитории
5. Таблицей доступных tools и resources

## Риски / Компромиссы

- [Конфиг не в репозитории — каждый разработчик настраивает вручную] → Подробная инструкция в README снижает трение
- [MCP-сервер должен быть запущен до подключения Cline] → Явно указать порядок в инструкции

## План миграции

1. Дополнить `README.md` разделом про MCP
2. Удалить `.cline/mcp.json` из репозитория (не работает для VS Code расширения)
3. Проверить подключение: запустить Docker Compose, добавить сервер через панель Cline

## Открытые вопросы
