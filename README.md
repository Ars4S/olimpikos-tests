# Тестовое задание ОЛИМПОКС — Selenium + C# + xUnit

Автотесты для каталога [olimpoks.ru](https://olimpoks.ru/): выгрузка видеоинструктажей и проверка пограничных значений поиска.

## Стек

- C# / .NET 9
- [Selenium WebDriver](https://www.selenium.dev/)
- [xUnit](https://xunit.net/)
- Паттерн **Page Object**

## Структура проекта

```
Olimpoks.sln
Olimpoks.Tests/
├── Pages/           # HomePage, CatalogPage, BasePage
├── Models/          # CourseInfo
├── Helpers/         # CourseCipherParser
├── Infrastructure/  # WebDriverFactory
├── Tests/           # тесты xUnit
└── Output/          # courses.txt (создаётся тестом)
```

## Требования

- [.NET SDK 8+](https://dotnet.microsoft.com/download)
- Google Chrome (браузер)

## Запуск тестов

```bash
cd /Users/arsenbabayan/Documents/test
dotnet test
```

Headless-режим (без окна браузера):

```bash
HEADLESS=1 dotnet test
```

## Тесты

| Тест | Описание |
|------|----------|
| `Export_VideoInstructions_ForWorkersAndInstruction_ToFile` | Главная → **Решения → Охрана труда** → фильтры **Рабочие** + **Инструктаж** → секция **Видеоинструктажи** → файл `Output/courses.txt` в формате `Название (Шифр) – N` |
| `Search_WithSingleCharacter_DoesNotReduceVisibleCourses` | Пограничное значение: 1 символ в поиске (минимум на сайте — 2) |
| `Search_WithNonExistentLongQuery_ReturnsZeroCourses` | Пограничное значение: длинный несуществующий запрос → 0 курсов |

## Публикация на GitHub

```bash
git init
git add .
git commit -m "Add Olimpoks catalog Selenium tests"
git branch -M main
git remote add origin https://github.com/ВАШ_ЛОГИН/olimpoks-tests.git
git push -u origin main
```

Ссылку на репозиторий отправьте работодателю вместе с кратким описанием запуска (`dotnet test`).
