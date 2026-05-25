## Зачем

Репозиторий не имеет зафиксированных архитектурных решений по структуре директорий и code style. Фиксация этих решений в ADR обеспечит единообразие кода и документации при росте проекта и смене контекста агента.

## Что меняется

- Создать ADR-0002: структура директорий проекта (`/docs`, `/apps`, `/infrastructure`, `/homework`)
- Создать ADR-0003: code style для C# (табы, базовые правила именования)
- Создать ADR-0004: code style для Markdown (MD009, MD012, MD022, MD032, MD034, MD047)
- Обновить шаблоны OpenSpec (`templates/*.md`) под Markdown code style из ADR-0004

## Возможности

### Новые возможности

- `project-structure`: Структура директорий репозитория
- `csharp-code-style`: Правила code style для C#
- `markdown-code-style`: Правила code style для Markdown

### Изменённые возможности

_(нет)_

## Влияние

- `docs/adr/general/` — новые ADR-файлы (0002, 0004)
- `docs/adr/dotnet/` — новый ADR-файл (0003)
- `docs/adr/README.md` — обновление индекса
- `openspec/schemas/spec-driven-with-adr/templates/` — обновление шаблонов под Markdown code style
