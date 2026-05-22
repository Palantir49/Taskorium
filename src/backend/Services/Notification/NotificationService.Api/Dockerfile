FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Services/Notification/NotificationService.Api/NotificationService.Api.csproj", "Services/Notification/NotificationService.Api/"]
COPY ["Shared/Taskorium.ServiceDefaults/Taskorium.ServiceDefaults.csproj", "Shared/Taskorium.ServiceDefaults/"]
COPY ["Services/Notification/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "Services/Notification/NotificationService.Infrastructure/"]
COPY ["Shared/Taskorium.MessageBus/Taskorium.MessageBus.csproj", "Shared/Taskorium.MessageBus/"]
COPY ["Services/Notification/NotificationService.Application/NotificationService.Application.csproj", "Services/Notification/NotificationService.Application/"]
COPY ["Shared/Taskorium.IntegrationEvents/Taskorium.IntegrationEvents.csproj", "Shared/Taskorium.IntegrationEvents/"]
COPY ["Services/Notification/NotificationService.Domain/NotificationService.Domain.csproj", "Services/Notification/NotificationService.Domain/"]
RUN dotnet restore "Services/Notification/NotificationService.Api/NotificationService.Api.csproj"
COPY . .
WORKDIR "/src/Services/Notification/NotificationService.Api"
RUN dotnet build "./NotificationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NotificationService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.Api.dll"]
