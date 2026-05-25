## 1. Правила Cline (.clinerules/)

- [x] 1.1 Создать `.clinerules/language.md` — язык взаимодействия русский
- [x] 1.2 Создать `.clinerules/prompts.md` — формат промтов (Роль/Контекст/Задача/Требования)
- [x] 1.3 Создать `.clinerules/memory-bank.md` — правило чтения и обновления memory bank
- [x] 1.4 Создать `.clinerules/adr.md` — правило работы с ADR (читать индекс, писать в docs/adr/)

## 2. Игнорирование файлов

- [x] 2.1 Создать `.clineignore` — bin/, obj/, *.snk, *.png, *.svg, *.jpg, *.ico
- [x] 2.2 Создать `.gitignore` — .NET артефакты (bin/, obj/, .vs/), секреты (.env, *.user)

## 3. Memory bank

- [x] 3.1 Выполнить команду: `init memory bank in folder /docs/ai-memory-bank`

## 4. Система ADR

- [x] 4.1 Убедиться, что `docs/adr/README.md` существует и содержит ADR 0001
- [x] 4.2 Убедиться, что `docs/adr/general/0001-adr-structure.md` существует

## 5. Обновление OpenSpec схемы

- [x] 5.1 Обновить `openspec/schemas/spec-driven-with-adr/schema.yaml` — путь ADR: `../../../docs/adr/**/*.md`
- [x] 5.2 Обновить `openspec/schemas/spec-driven-with-adr/templates/adr.md` — русский язык, компактный стиль
- [x] 5.3 Обновить `openspec/schemas/spec-driven-with-adr/templates/proposal.md` — русский язык
- [x] 5.4 Обновить `openspec/schemas/spec-driven-with-adr/templates/design.md` — русский язык
- [x] 5.5 Обновить `openspec/schemas/spec-driven-with-adr/templates/spec.md` — русский язык
- [x] 5.6 Обновить `openspec/schemas/spec-driven-with-adr/templates/tasks.md` — русский язык

## 6. Обновление skills и workflows

- [x] 6.1 Обновить `.clinerules/skills/openspec-propose/SKILL.md` — заменить `<repo>/adr/` на `<repo>/docs/adr/`
- [x] 6.2 Обновить `.clinerules/skills/openspec-apply-change/SKILL.md` — заменить `<repo>/adr/` на `<repo>/docs/adr/`
- [x] 6.3 Обновить `.clinerules/skills/openspec-archive-change/SKILL.md` — заменить `<repo>/adr/` на `<repo>/docs/adr/`
- [x] 6.4 Обновить `.clinerules/workflows/` — заменить ссылки на путь ADR
