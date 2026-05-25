## ADDED Requirements

### Requirement: CI workflow при push и pull request

Репозиторий SHALL содержать GitHub Actions workflow, запускающийся при каждом `push` и `pull_request` в любую ветку.

Workflow SHALL выполнять последовательно:

1. Сборку сервиса
2. Запуск тестов
3. Проверку code coverage

#### Scenario: Запуск при push

- **WHEN** разработчик делает `git push` в любую ветку
- **THEN** GitHub Actions автоматически запускает CI workflow

#### Scenario: Запуск при pull request

- **WHEN** создаётся или обновляется pull request
- **THEN** GitHub Actions автоматически запускает CI workflow

### Requirement: Сборка сервиса в CI

CI workflow SHALL собирать все .NET сервисы репозитория командой `dotnet build`.

#### Scenario: Успешная сборка

- **WHEN** код компилируется без ошибок
- **THEN** шаг сборки завершается успешно, workflow продолжается

#### Scenario: Ошибка сборки

- **WHEN** код содержит ошибки компиляции
- **THEN** шаг сборки завершается с ошибкой, workflow останавливается

### Requirement: Запуск тестов в CI

CI workflow SHALL запускать все тесты командой `dotnet test`.

#### Scenario: Все тесты прошли

- **WHEN** все тесты завершаются успешно
- **THEN** шаг тестов завершается успешно, workflow продолжается

#### Scenario: Тест упал

- **WHEN** хотя бы один тест завершается с ошибкой
- **THEN** шаг тестов завершается с ошибкой, workflow останавливается

### Requirement: Проверка code coverage в CI

CI workflow SHALL проверять, что code coverage составляет не менее 80%. При несоответствии workflow SHALL завершаться с ошибкой.

#### Scenario: Coverage соответствует порогу

- **WHEN** line coverage ≥ 80%
- **THEN** шаг проверки coverage завершается успешно

#### Scenario: Coverage ниже порога

- **WHEN** line coverage < 80%
- **THEN** шаг проверки coverage завершается с ошибкой, workflow останавливается

### Requirement: Блокировка PR при падении CI

Pull request SHALL быть заблокирован для слияния, если CI workflow завершился с ошибкой (сборка, тесты или coverage).

#### Scenario: PR заблокирован

- **WHEN** CI workflow завершается с ошибкой
- **THEN** кнопка "Merge" в GitHub PR недоступна до исправления ошибок

#### Scenario: PR разблокирован

- **WHEN** CI workflow завершается успешно
- **THEN** PR доступен для слияния (при выполнении остальных условий)

### Requirement: Расположение workflow файлов

GitHub Actions workflow файлы SHALL располагаться в `.github/workflows/`.

#### Scenario: Расположение CI workflow

- **WHEN** разработчик открывает `.github/workflows/`
- **THEN** присутствует файл CI workflow для .NET сервисов (например, `dotnet-ci.yml`)
