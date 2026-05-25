## ADDED Requirements

### Requirement: Список контрактов

Сервис SHALL предоставлять эндпоинт `GET /api/contracts`, возвращающий список всех доступных swagger-контрактов из источника данных.

Ответ SHALL содержать для каждого контракта: имя файла, заголовок (из поля `info.title`), версию (из поля `info.version`), описание (из поля `info.description`).

#### Scenario: Успешное получение списка

- **WHEN** клиент отправляет `GET /api/contracts`
- **THEN** сервис возвращает `200 OK` с JSON-массивом объектов `{ "name": string, "title": string, "version": string, "description": string }`

#### Scenario: Пустой каталог

- **WHEN** в папке источника нет swagger-файлов
- **THEN** сервис возвращает `200 OK` с пустым массивом `[]`

### Requirement: Получение контракта по имени

Сервис SHALL предоставлять эндпоинт `GET /api/contracts/{name}`, возвращающий содержимое конкретного swagger-файла.

#### Scenario: Успешное получение файла

- **WHEN** клиент отправляет `GET /api/contracts/{name}` с существующим именем файла
- **THEN** сервис возвращает `200 OK` с содержимым YAML-файла, `Content-Type: application/yaml`

#### Scenario: Контракт не найден

- **WHEN** клиент отправляет `GET /api/contracts/{name}` с несуществующим именем
- **THEN** сервис возвращает `404 Not Found`

#### Scenario: Попытка path traversal

- **WHEN** клиент передаёт имя с `..` или абсолютным путём
- **THEN** сервис возвращает `400 Bad Request`

### Requirement: Поиск контрактов

Сервис SHALL предоставлять эндпоинт `GET /api/contracts/search?q={query}`, выполняющий поиск по имени файла и полям `info.title`, `info.description`.

#### Scenario: Успешный поиск с результатами

- **WHEN** клиент отправляет `GET /api/contracts/search?q=asset`
- **THEN** сервис возвращает `200 OK` с JSON-массивом контрактов, в которых строка `asset` встречается в имени, title или description

#### Scenario: Поиск без результатов

- **WHEN** строка запроса не совпадает ни с одним контрактом
- **THEN** сервис возвращает `200 OK` с пустым массивом `[]`

#### Scenario: Пустой запрос

- **WHEN** параметр `q` отсутствует или пустой
- **THEN** сервис возвращает `400 Bad Request`

### Requirement: CORS

Сервис SHALL поддерживать CORS, разрешая запросы с любого origin, чтобы браузерные клиенты (в том числе Swagger UI на другом порту) могли обращаться к API.

#### Scenario: Запрос из Swagger UI

- **WHEN** браузер отправляет запрос к `GET /api/contracts/{name}` с origin `http://localhost:5100`
- **THEN** сервис возвращает ответ с заголовком `Access-Control-Allow-Origin: *`

### Requirement: Contract-first разработка

Сервис SHALL иметь swagger-контракт своего REST API, расположенный в `docs/contracts/wiki/wiki-api.yaml`, до начала реализации.

#### Scenario: Контракт существует до реализации

- **WHEN** разработчик начинает реализацию wiki REST API
- **THEN** файл `docs/contracts/wiki/wiki-api.yaml` уже существует и описывает все эндпоинты
