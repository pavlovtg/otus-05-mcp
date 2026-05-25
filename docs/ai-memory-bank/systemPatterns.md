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
