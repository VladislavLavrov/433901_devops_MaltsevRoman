# 8_Calculator — интеграция с MariaDB

В проект добавлены `DataInputVariant`, `CalculatorContext`, подключение EF Core к MariaDB и сохранение вычислений в `CalculatorController`.

## Перед запуском

1. Убедитесь, что доступен сервер MariaDB и создана база данных `myWebCalculatorDb`.
2. Проверьте строку подключения в `appsettings.json` или `appsettings.Development.json`. В Docker-сети `mariadb` должно быть именем сервиса базы данных. При локальном запуске обычно указывают `Server=localhost;Port=3306`.
3. Пароль и другие параметры подключения измените на свои, если они отличаются от учебного примера.
4. Запустите проект. `EnsureCreated()` создаст таблицу для истории, если её ещё нет. Базу данных на сервере нужно создать заранее.

Пакет `Pomelo.EntityFrameworkCore.MySql` добавлен для подключения к MariaDB с EF Core 8.

## Что изменено

- `Program.cs`: регистрация `CalculatorContext` и MariaDB.
- `Controllers/CalculatorController.cs`: вычисления и запись операции в БД.
- `Data/DataInputVariant.cs`: модель записи истории.
- `Data/CalculatorContext.cs`: контекст Entity Framework Core.
- `appsettings*.json`: строки подключения.

Обратите внимание: `EnsureCreated()` предназначен для простого учебного проекта. Если база уже управляется миграциями EF Core, вместо него следует использовать миграции.
