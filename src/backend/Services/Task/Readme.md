# 🏗️ Структура решения

TaskManagement.sln  
├── Services/  
│   └── Task/  
│       ├── TaskService.Api/                    # Web API слой (Презентационный)  
│       ├── TaskService.Application/            # Слой приложения (CQRS/Use Cases)  
│       ├── TaskService.Contracts/              # DTO и API контракты  
│       ├── TaskService.Domain/                 # Доменный слой (Core/Business Logic)  
│       └── TaskService.Infrastructure/         # Инфраструктурный слой  
└── Solution Items/  
├── .editorconfig                          # Стиль кода  
├── Directory.Packages.props               # Централизованное управление пакетами  
├── Directory.Build.props                  # Общие настройки сборки  
├── NuGet.config                           # Конфигурация NuGet  
└── global.json                            # Версия .NET SDK  


## 📦 Подробная структура проектов
## **TaskService** - проект управления задачами
### 1. TaskService.Api - Презентационный слой
Назначение: Обработка HTTP запросов, аутентификация/авторизация, маппинг DTO, валидация входящих данных.

Структура каталогов:

TaskService.Api/  
├── Controllers/                              # API контроллеры (TaskController, UserController, etc.)  
├── Middleware/                               # Кастомные middleware (ExceptionHandling, Logging)  
├── Extension/                                # Методы расширения API  
├── Program.cs                                # Точка входа с настройкой DI  
├── appsettings.json                          # Конфигурация приложения    
├── appsettings.Development.json              # Конфигурация для разработки    
└── Properties/ 

### 2. TaskService.Application - Слой приложения (CQRS)
Назначение: Оркестрация бизнес-сценариев, координация доменных объектов, реализация CQRS паттерна, валидация бизнес-правил.

Структура каталогов:


TaskService.Application/  
├── Commands/                                # Команды (Write операции)  
│   ├── TaskCommands/  
│   │   ├── CreateTaskCommand.cs  
│   │   ├── UpdateTaskCommand.cs  
│   │   ├── AssignTaskCommand.cs  
│   │   └── ChangeTaskStatusCommand.cs  
│   ├── UserCommands/  
├── Queries/                                 # Запросы (Read операции)  
│   ├── TaskQueries/  
│   ├── UserQueries/    
│   └── Handlers/  
│       ├── TaskQueryHandlers/  
│       └── UserQueryHandlers/  
├── Common/                                  # Общие компоненты CQRS  
│   ├── Interfaces/  
│   │   ├── ICommand.cs  
│   │   ├── IQuery.cs  
│   │   ├── ICommandHandler.cs  
│   │   ├── IQueryHandler.cs  
│   │   ├── ICommandDispatcher.cs  
│   │   └── IQueryDispatcher.cs  
│   ├── Dispatchers/                         # Реализация диспетчеров    
│   ├── Behaviors/                           # Pipeline Behaviors  
│   ├── Exceptions/                          # Исключения приложения  
│   └── Models/                              # Общие модели  
├── Validators/                              # FluentValidation валидаторы  
└── Extensions                   # Регистрация зависимостей Application слоя   


### 3. TaskService.Contracts - Слой контрактов
Назначение: Определение API контрактов, DTO для передачи данных между слоями

Структура каталогов:

TaskService.Contracts/  
├── Requests/                                # Входящие DTO (от клиента к серверу)   
├── Responses/                               # Исходящие DTO (от сервера к клиенту)   
├── Common/                                  # Общие типы     


### 4. TaskService.Domain - Доменный слой
Назначение: Чистая бизнес-логика, доменные модели.

TaskService.Domain/  
├── Entities/                                # Агрегаты и сущности  
├── ValueObjects/                            # Объекты-значения     
├── Enums/                                   # Перечисления   
├── Events/                                  # Доменные события   
│   ├── TaskCreatedEvent.cs  
│   ├── TaskAssignedEvent.cs  
│   ├── TaskStatusChangedEvent.cs  
│   └── IDomainEvent.cs  
├── Exceptions/                              # Доменные исключения  
│   ├── DomainException.cs                   # Базовое исключение  
│   ├── TaskNotFoundException.cs  
│   ├── InvalidTaskStateException.cs  
│   └── BusinessRuleViolationException.cs  
└── Extensions                               # Регистрация зависимостей  


### 5. TaskService.Infrastructure - Инфраструктурный слой  
Назначение: Реализация деталей: база данных, внешние сервисы, кэширование, логирование.

Структура каталогов:  

TaskService.Infrastructure/  
├── Persistence/                            # Доступ к данным  
│   ├── Context/  
│   │   ├── TaskDbContext.cs                # Основной DbContext  
│   ├── Configurations/                     # Конфигурации EF Core  
│   ├── Migrations/                         # Миграции базы данных  
│   │   ├── 20240101000000_InitialCreate.cs  
│   │   └── TaskDbContextModelSnapshot.cs  
├── Services/                                # Инфраструктурные сервисы  
│   ├── FileStorage/  
│   │   ├── MinioFileStorageService.cs  
│   │   └── LocalFileStorageService.cs  
│   └── Caching/   
├── Logging/                                 # Логирование   
└── Extensions                  # Регистрация зависимостей инфраструктуры  

## **StorageService** - проект для взаимодействия с хранилищем