## ADDED Requirements

### Requirement: Инициализация memory bank

Memory bank SHALL быть инициализирован в папке `docs/ai-memory-bank/` со стандартными файлами.

#### Scenario: Структура memory bank

- **WHEN** memory bank инициализирован
- **THEN** папка `docs/ai-memory-bank/` содержит файлы: `projectbrief.md`, `productContext.md`, `activeContext.md`, `systemPatterns.md`, `techContext.md`, `progress.md`

### Requirement: Чтение memory bank при каждой задаче

Cline SHALL читать файлы memory bank в начале каждой новой задачи.

#### Scenario: Начало задачи

- **WHEN** пользователь начинает новую задачу
- **THEN** Cline читает все файлы из `docs/ai-memory-bank/` перед началом работы

### Requirement: Обновление memory bank

Cline SHALL обновлять memory bank при изменении контекста проекта.

#### Scenario: Обновление после значимых изменений

- **WHEN** в проекте произошли значимые изменения (новые решения, изменение архитектуры, прогресс)
- **THEN** Cline обновляет соответствующие файлы memory bank

### Requirement: Язык и формат memory bank — русский, компактный

Файлы memory bank SHALL быть написаны на русском языке в компактном стиле: буллеты, одно предложение вместо абзаца, без вводных фраз.

#### Scenario: Создание файла memory bank

- **WHEN** агент создаёт или обновляет файл memory bank
- **THEN** содержимое написано на русском языке, буллетами, без вводных фраз
