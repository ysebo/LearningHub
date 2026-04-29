# LearningHub API

LearningHub — учебный проект системы управления обучением, построенный на ASP.NET Core Web API с Clean Architecture.

## Быстрый старт

```powershell
dotnet run --project LearningHub.API
```

Swagger откроется по адресу:

`http://localhost:5118/swagger`

## Структура проекта

- `LearningHub.API` — контроллеры, Swagger, middleware, настройка запуска
- `LearningHub.Application` — DTO, интерфейсы, бизнес-логика, сервисы
- `LearningHub.Infrastructure` — EF Core, PostgreSQL, репозитории, миграции
- `LearningHub.Domain` — доменные сущности и перечисления

## Идея проекта

Представь проект как онлайн-школу:

- `Category` — направление (например, Программирование)
- `Course` — конкретный курс в этом направлении
- `Student` — студент
- `Enrollment` — запись студента на курс

## Основные эндпоинты

**Categories**
- `GET /api/categories` — список всех категорий
- `GET /api/categories/{id}` — категория с курсами
- `POST /api/categories` — создать категорию
- `PUT /api/categories/{id}` — обновить категорию
- `DELETE /api/categories/{id}` — удалить категорию

**Students**
- `GET /api/students` — список всех студентов
- `GET /api/students/{id}` — студент с курсами
- `POST /api/students` — создать студента
- `PUT /api/students/{id}` — обновить студента
- `DELETE /api/students/{id}` — удалить студента

## База данных

Используется PostgreSQL. Строка подключения в `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=learningHub;Username=postgres;Password=1234"
}
```

## Миграции

Создать новую миграцию:
```powershell
dotnet ef migrations add MigrationName --project LearningHub.Infrastructure --startup-project LearningHub.API
```

Применить миграции:
```powershell
dotnet ef database update --project LearningHub.Infrastructure --startup-project LearningHub.API
```

## Стек технологий

- ASP.NET Core Web API (.NET 10)
- Entity Framework Core (Code First)
- PostgreSQL
- Swagger / OpenAPI
- Dependency Injection (встроенный)

