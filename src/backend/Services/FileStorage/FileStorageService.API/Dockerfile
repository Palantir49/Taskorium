FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Services/FileStorage/FileStorageService.API/FileStorageService.API.csproj", "Services/FileStorage/FileStorageService.API/"]
COPY ["Shared/Taskorium.ServiceDefaults/Taskorium.ServiceDefaults.csproj", "Shared/Taskorium.ServiceDefaults/"]
COPY ["Services/FileStorage/FileStorageService.Application/FileStorageService.Application.csproj", "Services/FileStorage/FileStorageService.Application/"]
COPY ["Services/FileStorage/FileStorageService.Domain/FileStorageService.Domain.csproj", "Services/FileStorage/FileStorageService.Domain/"]
COPY ["Services/FileStorage/FileStorageService.Infrastructure/FileStorageService.Infrastructure.csproj", "Services/FileStorage/FileStorageService.Infrastructure/"]
COPY ["Shared/Taskorium.Protos/Taskorium.Protos.csproj", "Shared/Taskorium.Protos/"]
RUN dotnet restore "Services/FileStorage/FileStorageService.API/FileStorageService.API.csproj"
COPY . .
WORKDIR "/src/Services/FileStorage/FileStorageService.API"
RUN dotnet build "./FileStorageService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FileStorageService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileStorageService.API.dll"]
