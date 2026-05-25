## ADDED Requirements

### Requirement: Файл .gitignore для .NET

Репозиторий SHALL содержать `.gitignore` со стандартными исключениями для .NET проекта.

#### Scenario: Игнорирование артефактов сборки

- **WHEN** разработчик делает коммит
- **THEN** директории `bin/`, `obj/`, `.vs/` не попадают в репозиторий

### Requirement: Секреты не коммитятся

`.gitignore` SHALL включать файлы с секретами: `.env`, `*.user`, `appsettings.*.json` (кроме `appsettings.json`).

#### Scenario: Защита секретов

- **WHEN** разработчик делает коммит
- **THEN** файлы `.env` и локальные настройки не попадают в репозиторий
