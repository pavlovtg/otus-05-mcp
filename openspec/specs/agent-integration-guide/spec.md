## Requirements

### Requirement: Инструкция по запуску MCP и подключению Cline в README.md

`README.md` SHALL содержать раздел с инструкцией по запуску MCP-сервера и подключению агента Cline, включая описание доступных tools и resources.

#### Scenario: Инструкция по запуску присутствует

- **WHEN** разработчик открывает `README.md`
- **THEN** находит раздел про MCP-сервер с командой запуска через Docker Compose

#### Scenario: Инструкция по подключению Cline присутствует

- **WHEN** разработчик читает раздел про MCP в `README.md`
- **THEN** находит пошаговую инструкцию (не более 5 шагов) по подключению Cline к MCP-серверу

#### Scenario: Описание tools и resources присутствует

- **WHEN** разработчик читает раздел про MCP в `README.md`
- **THEN** находит список доступных tools (`list_apis`, `search_apis`) и resources (`api://{name}`) с описанием
