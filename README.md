***

# 📚 Books API

**Books API** — это RESTful веб-сервис для управления библиотекой книг, написанный на **ASP.NET Core (.NET 8)**. Проект демонстрирует использование современных архитектурных подходов, таких как Clean Architecture (N-Tier), Repository & Unit of Work паттерны, а также контейнеризацию с помощью Docker.

## 🚀 Технологический стек

- **Фреймворк:** .NET 8 / ASP.NET Core Web API
- **База данных:** PostgreSQL
- **ORM:** Entity Framework Core
- **Архитектура:** Controller-Service-Repository, Unit of Work
- **Аутентификация/Авторизация:** JWT (JSON Web Tokens)
- **Валидация:** FluentValidation
- **Контейнеризация:** Docker, Docker Compose
- **Документация API:** OpenAPI (Swagger) с использованием [Scalar](https://github.com/scalar/scalar) темы
- **Обработка ошибок:** Custom Global Exception Middleware

## 🏗️ Архитектура проекта

Проект разделен на логические слои для обеспечения слабой связности и удобства тестирования:
- `Controllers/` — обработка входящих HTTP-запросов и возврат ответов.
- `Services/` — слой бизнес-логики.
- `Repositories/` — абстракция над Entity Framework Core для доступа к данным.
- `DTOs/` — Data Transfer Objects для защиты доменных моделей и валидации входящих данных.
- `Models/` — доменные сущности (Entity) базы данных.
- `Middleware/` — глобальный перехват и обработка исключений (`NotFoundException`, `BadRequestException`).

## 🛠️ Локальный запуск (Docker)

Самый быстрый способ запустить проект — использовать Docker Compose. В этом случае поднимется приложение и база данных PostgreSQL.

**Требования:**
- [Docker](https://www.docker.com/products/docker-desktop) и Docker Compose

**Шаги для запуска:**
1. Склонируйте репозиторий:
   ```bash
   git clone <url-вашего-репозитория>
   cd book-api/BooksApi
   ```
2. Выполните команду сборки и запуска:
   ```bash
   docker-compose up --build
   ```
3. API будет доступно по адресу: `http://localhost:5000`
4. Документация API (Scalar UI): `http://localhost:5000/scalar/v1`

*(База данных автоматически накатит миграции при старте приложения).*

## 🔑 Аутентификация

Большинство эндпоинтов защищены авторизацией. Для доступа к ним необходимо:
1. Зарегистрироваться / Войти через эндпоинты `Auth` (в разработке/если применимо).
2. Получить JWT-токен.
3. Передать токен в заголовке запроса: `Authorization: Bearer <твой_токен>`.

## 📡 Основные API Эндпоинты

**Books (`/api/books`)**
- `GET /api/books` — получить список всех книг
- `GET /api/books/{id}` — получить книгу по ID
- `GET /api/books/search?query={text}` — поиск книг
- `GET /api/books/genre/{genreId}` — получить книги по жанру
- `POST /api/books` — добавить новую книгу (Требуется Авторизация)
- `PUT /api/books/{id}` — обновить информацию о книге (Требуется Авторизация)
- `DELETE /api/books/{id}` — удалить книгу (Требуется роль **Admin**)

**Genres (`/api/genres`)**
- `GET /api/genres` — получить все жанры

## 📝 Планы по развитию (Roadmap)
- [ ] Добавление пагинации для списков книг.
- [ ] Написание Дополнительного Unit-тестов (xUnit + Moq).
- [ ] Интеграция MediatR (CQRS).

***

*** 