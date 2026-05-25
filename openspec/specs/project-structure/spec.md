## ADDED Requirements

### Requirement: Структура директорий репозитория

Репозиторий SHALL использовать следующую структуру директорий верхнего уровня:

- `/docs` — документация проекта
- `/docs/ai-memory-bank` — memory bank AI-агента (6 файлов)
- `/docs/adr` — архитектурные решения (ADR)
- `/apps` — приложения (MCP-сервер и другие исполняемые компоненты)
- `/infrastructure` — инфраструктурный код (Docker, Kubernetes, CI/CD)
- `/homework` — учебные задания OTUS

#### Scenario: Новый компонент приложения

- **WHEN** разработчик создаёт новый исполняемый компонент
- **THEN** компонент размещается в `/apps/<component-name>/`

#### Scenario: Новый ADR

- **WHEN** принимается архитектурное решение
- **THEN** ADR создаётся в `/docs/adr/<domain>/NNNN-kebab-title.md`

#### Scenario: Инфраструктурный код

- **WHEN** добавляется конфигурация развёртывания
- **THEN** файлы размещаются в `/infrastructure/`
