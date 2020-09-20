#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Services/MarsRover/MarsRover.Api/MarsRover.Api.csproj", "src/Services/MarsRover/MarsRover.Api/"]
COPY ["src/Infrastructure/EventBus/Hb.MarsRover.Infrastructure.EventBus/Hb.MarsRover.Infrastructure.EventBus.csproj", "src/Infrastructure/EventBus/Hb.MarsRover.Infrastructure.EventBus/"]
COPY ["src/Services/MarsRover/MarsRover.Domain/MarsRover.Domain.csproj", "src/Services/MarsRover/MarsRover.Domain/"]
COPY ["src/Domain/Hb.MarsRover.Domain/Hb.MarsRover.Domain.csproj", "src/Domain/Hb.MarsRover.Domain/"]
COPY ["src/DataAccess/MarsRover.DataAccess.EntityFramework/Hb.MarsRover.DataAccess.EntityFramework.csproj", "src/DataAccess/MarsRover.DataAccess.EntityFramework/"]
COPY ["src/DataAccess/Hb.MarsRover.DataAccess.Core/Hb.MarsRover.DataAccess.Core.csproj", "src/DataAccess/Hb.MarsRover.DataAccess.Core/"]
COPY ["src/Infrastructure/Core/Hb.MarsRover.Infrastructure.Core/Hb.MarsRover.Infrastructure.Core.csproj", "src/Infrastructure/Core/Hb.MarsRover.Infrastructure.Core/"]
COPY ["src/Infrastructure/Logging/MarsRover.Infrastructure.Logging/MarsRover.Infrastructure.Logging.csproj", "src/Infrastructure/Logging/MarsRover.Infrastructure.Logging/"]
COPY ["src/Infrastructure/Configuration/Hb.MarsRover.Infrastructure.Configuration/Hb.MarsRover.Infrastructure.Configuration.csproj", "src/Infrastructure/Configuration/Hb.MarsRover.Infrastructure.Configuration/"]
COPY ["src/Infrastructure/Core/Hb.MarsRover.Infrastructure.Core.Exception/Hb.MarsRover.Infrastructure.Core.Exception.csproj", "src/Infrastructure/Core/Hb.MarsRover.Infrastructure.Core.Exception/"]
COPY ["src/Services/MarsRover/MarsRover.Infrastructure/MarsRover.Infrastructure.csproj", "src/Services/MarsRover/MarsRover.Infrastructure/"]
COPY ["src/Infrastructure/EventBus/Hb.MarsRover.Infrastructure.EventBus.EFEventStore/Hb.MarsRover.Infrastructure.EventBus.EFEventStore.csproj", "src/Infrastructure/EventBus/Hb.MarsRover.Infrastructure.EventBus.EFEventStore/"]
COPY ["src/Infrastructure/EventBus/Hb.MarsRover.Infrastructure.EventBus.RabbitMQ/Hb.MarsRover.Infrastructure.EventBus.RabbitMQ.csproj", "src/Infrastructure/EventBus/Hb.MarsRover.Infrastructure.EventBus.RabbitMQ/"]
COPY ["src/Infrastructure/Core/Hb.MarsRover.Infrastructure.Core.Http/Hb.MarsRover.Infrastructure.Core.Http.csproj", "src/Infrastructure/Core/Hb.MarsRover.Infrastructure.Core.Http/"]
RUN dotnet restore "src/Services/MarsRover/MarsRover.Api/MarsRover.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/MarsRover/MarsRover.Api"
RUN dotnet build "MarsRover.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarsRover.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarsRover.Api.dll"]