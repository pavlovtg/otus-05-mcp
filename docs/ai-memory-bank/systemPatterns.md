# System Patterns

## Архитектурные паттерны

- MCP-сервер: stdio или HTTP транспорт (решение предстоит)
- OpenSpec workflow: `spec-driven-with-adr` — proposal → specs → design → adr → tasks

## Правила работы с ADR

- ADR хранятся в `docs/adr/<domain>/`, кросс-доменные — в `docs/adr/general/`
- Индекс: `docs/adr/README.md` (одна строка на ADR)
- ADR иммутабельны; пересмотр — новый ADR с `supersedes`

## Правила работы с кодом

- Язык: C# (.NET)
- Артефакты сборки игнорируются: `bin/`, `obj/`, `.vs/`

## Code style C# (ADR-0003)

- Отступы: табы
- Именование: `PascalCase` для классов/методов/свойств, `camelCase` для переменных, `_camelCase` для приватных полей, `IPascalCase` для интерфейсов
- Один публичный тип на файл; имя файла = имя типа

## Code style Markdown (ADR-0004)

- MD022: заголовки окружены пустыми строками
- MD047: файл заканчивается одним `\n`
- MD012: не более одной пустой строки подряд
- MD032: списки окружены пустыми строками
- MD034: URL в угловых скобках `<url>`
- MD009: нет trailing spaces
