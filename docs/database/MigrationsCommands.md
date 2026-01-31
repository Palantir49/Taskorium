**Add migartion**: dotnet ef migrations add AddUserFullName --project .\Services\Task\TaskService.Infrastructure\TaskService.Infrastructure.csproj --startup-project .\Services\Task\TaskService.Api\TaskService.Api.csproj
**Update databas**: dotnet ef database update --verbose --project
.\Services\Task\TaskService.Infrastructure\TaskService.Infrastructure.csproj --startup-project
.\Services\Task\TaskService.Api\TaskService.Api.csproj