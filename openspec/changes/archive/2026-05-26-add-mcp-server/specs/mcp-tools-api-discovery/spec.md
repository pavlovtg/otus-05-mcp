## ADDED Requirements

### Requirement: Tool list_apis

MCP-сервер SHALL предоставлять tool `list_apis`, возвращающий список всех доступных API.

Tool SHALL возвращать структурированный результат: массив объектов с полями `name`, `title`, `version`, `description`.

#### Scenario: Получение списка API

- **WHEN** MCP-клиент вызывает tool `list_apis` без параметров
- **THEN** сервер возвращает массив со всеми доступными API

#### Scenario: Пустой каталог

- **WHEN** каталог API не содержит ни одного контракта
- **THEN** tool возвращает пустой массив

### Requirement: Tool search_apis

MCP-сервер SHALL предоставлять tool `search_apis` с обязательным параметром `query` (строка).

Tool SHALL возвращать отфильтрованный список API, соответствующих поисковому запросу, в том же формате что и `list_apis`.

#### Scenario: Поиск по имени

- **WHEN** MCP-клиент вызывает tool `search_apis` с параметром `query`
- **THEN** сервер возвращает список API, чьё имя или описание содержит строку запроса

#### Scenario: Нет результатов

- **WHEN** поисковый запрос не соответствует ни одному API
- **THEN** tool возвращает пустой массив
