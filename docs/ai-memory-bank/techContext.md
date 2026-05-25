# Tech Context

## Стек

- Язык: C# / .NET 10
- Протокол: Model Context Protocol (MCP)
- AI-ассистент: Cline (VS Code)
- Web-фреймворк: ASP.NET Core (wiki-api)
- YAML-парсер: YamlDotNet 18.0.0
- Тестирование: xUnit + Microsoft.AspNetCore.Mvc.Testing + coverlet
- Контейнеризация: Docker + Docker Compose
- CI: GitHub Actions

## Инструменты

- OpenSpec CLI — управление изменениями (proposal → specs → design → tasks)
- Git — контроль версий
- VS Code — IDE
- dotnet CLI — сборка, тесты, coverage

## Зависимости

- .NET SDK 10.0
- YamlDotNet 18.0.0
- Microsoft.AspNetCore.Mvc.Testing 10.0.8
- coverlet.collector 10.0.1
- swaggerapi/swagger-ui (Docker образ)
- MCP SDK для .NET (TBD)

## Конфигурация

- `.clinerules/` — правила для Cline
- `.clineignore` — файлы, скрытые от агента
- `.gitignore` — стандартный для .NET
- `openspec/schemas/spec-driven-with-adr/` — схема OpenSpec
- `CONTRACTS_PATH` — переменная окружения для пути к контрактам (default: `./content`)
