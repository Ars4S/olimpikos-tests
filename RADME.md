# Тестовое задание ОЛИМПОКС

Автотесты каталога [olimpoks.ru](https://olimpoks.ru/) на **Selenium + C# + xUnit** (Page Object).

## Запуск

```bash
git clone https://github.com/Ars4S/olimpikos-tests.git
cd olimpikos-tests
dotnet test
```

Нужны **.NET SDK 8+** и **Google Chrome**.

## Тесты

1. **Export_VideoInstructions_ForWorkersAndInstruction_ToFile** — фильтры «Рабочие» + «Инструктаж», секция «Видеоинструктажи», выгрузка в `Olimpoks.Tests/Output/courses.txt` (формат: `Название (Шифр) – N`).
2. **Search_WithSingleCharacter_DoesNotReduceVisibleCourses** — поиск из 1 символа (граница: минимум 2).
3. **Search_WithNonExistentLongQuery_ReturnsZeroCourses** — несуществующий запрос → 0 курсов.
