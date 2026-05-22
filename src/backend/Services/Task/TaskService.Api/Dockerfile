FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Services/Task/TaskService.Api/TaskService.Api.csproj", "Services/Task/TaskService.Api/"]
COPY ["Services/Task/TaskService.Domain/TaskService.Domain.csproj", "Services/Task/TaskService.Domain/"]
COPY ["Shared/Taskorium.Protos/Taskorium.Protos.csproj", "Shared/Taskorium.Protos/"]
COPY ["Shared/Taskorium.ServiceDefaults/Taskorium.ServiceDefaults.csproj", "Shared/Taskorium.ServiceDefaults/"]
COPY ["Services/Task/TaskService.Application/TaskService.Application.csproj", "Services/Task/TaskService.Application/"]
COPY ["Services/Task/TaskService.Contracts/TaskService.Contracts.csproj", "Services/Task/TaskService.Contracts/"]
COPY ["Services/Task/TaskService.Infrastructure/TaskService.Infrastructure.csproj", "Services/Task/TaskService.Infrastructure/"]
RUN dotnet restore "Services/Task/TaskService.Api/TaskService.Api.csproj"
COPY . .
WORKDIR "/src/Services/Task/TaskService.Api"
RUN dotnet build "./TaskService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskService.Api.dll"]
