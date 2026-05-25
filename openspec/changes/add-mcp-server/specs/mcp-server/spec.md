## ADDED Requirements

### Requirement: MCP-сервер

Сервис `apps/mcp/` SHALL реализовывать MCP-сервер, предоставляющий AI-агентам доступ к каталогу API внутренних сервисов.

Сервер SHALL регистрировать tools и resources согласно протоколу MCP.

#### Scenario: Обнаружение capabilities

- **WHEN** MCP-клиент подключается к серверу
- **THEN** сервер возвращает список доступных tools и resources

### Requirement: HTTP-транспорт MCP

MCP-сервер SHALL быть доступен по HTTP (Streamable HTTP транспорт MCP).

MCP-эндпоинт SHALL принимать запросы от MCP-клиентов по стандартному HTTP-протоколу.

#### Scenario: Подключение MCP-клиента по HTTP

- **WHEN** MCP-клиент (например, IDE-агент) настроен на URL сервера
- **THEN** клиент успешно устанавливает MCP-сессию и получает список capabilities
