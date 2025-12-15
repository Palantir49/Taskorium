FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Task/TaskService.Api/TaskService.Api.csproj", "Services/Task/TaskService.Api/"]
COPY ["Services/Task/TaskService.Contracts/TaskService.Contracts.csproj", "Services/Task/TaskService.Contracts/"]
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
