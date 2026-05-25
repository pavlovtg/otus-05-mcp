## Why

Репозиторий не настроен для совместной работы с AI-ассистентом Cline: отсутствуют правила взаимодействия, форматирования промтов, memory bank и базовые конфигурационные файлы. Настройка нужна сейчас, до начала основной разработки MCP-сервера.

## What Changes

- Создать `.clinerules/language.md` — правило языка взаимодействия (русский)
- Создать `.clinerules/prompts.md` — правило формата промтов (Роль/Контекст/Задача/Требования, экономия токенов)
- Создать `.clinerules/memory-bank.md` — правило чтения и обновления memory bank
- Создать `.clinerules/adr.md` — правило работы с ADR
- Создать `.clineignore` — игнорирование нерелевантных файлов агентом
- Создать `docs/ai-memory-bank/` — инициализация memory bank (6 файлов)
- Создать `docs/adr/README.md` — индекс ADR
- Создать `docs/adr/general/` — папка для общих ADR
- Создать `.gitignore` — стандартный для .NET проекта
- Обновить `openspec/schemas/spec-driven-with-adr/schema.yaml` — путь ADR → `docs/adr/`
- Обновить `openspec/schemas/spec-driven-with-adr/templates/adr.md` — шаблон ADR (русский, компактный)
- Обновить `openspec/schemas/spec-driven-with-adr/templates/proposal.md` — русский, компактный стиль
- Обновить `openspec/schemas/spec-driven-with-adr/templates/design.md` — русский, компактный стиль
- Обновить `openspec/schemas/spec-driven-with-adr/templates/spec.md` — русский, компактный стиль
- Обновить `openspec/schemas/spec-driven-with-adr/templates/tasks.md` — русский, компактный стиль
- Обновить skills/workflows — ссылки на путь ADR

## Capabilities

### New Capabilities

- `agent-language-rules`: Правила языка взаимодействия и формата промтов для Cline
- `agent-memory-bank`: Инициализация и правила работы с memory bank в `docs/ai-memory-bank/`
- `agent-ignore-rules`: Настройка `.clineignore` для экономии токенов
- `adr-system`: Система ADR в `docs/adr/` с индексом и правилами
- `git-config`: Базовая конфигурация git (`.gitignore`) для .NET проекта
- `openspec-adr-path`: Обновление пути ADR и шаблонов OpenSpec (русский, компактный стиль)

### Modified Capabilities

_(нет существующих спецификаций)_

## Impact

- `.clinerules/` — новые файлы правил для Cline
- `.clineignore` — новый файл
- `.gitignore` — новый файл (.NET)
- `docs/ai-memory-bank/` — новая директория с файлами memory bank
- `docs/adr/` — новая директория системы ADR
- `openspec/schemas/spec-driven-with-adr/schema.yaml` — изменение пути ADR
- `openspec/schemas/spec-driven-with-adr/templates/` — обновление шаблонов (adr, proposal, design, spec, tasks)
- `.clinerules/skills/` и `.clinerules/workflows/` — обновление ссылок на путь ADR
