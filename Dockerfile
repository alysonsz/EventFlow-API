FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8079

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["EventFlow.Presentation/EventFlow.Presentation.csproj", "EventFlow.Presentation/"]
COPY ["EventFlow.Application/EventFlow.Application.csproj", "EventFlow.Application/"]
COPY ["EventFlow.Core/EventFlow.Core.csproj", "EventFlow.Core/"]
COPY ["EventFlow.Infrastructure/EventFlow.Infrastructure.csproj", "EventFlow.Infrastructure/"]

RUN dotnet restore "EventFlow.Presentation/EventFlow.Presentation.csproj"

COPY . .
WORKDIR "/src/EventFlow.Presentation"

RUN dotnet build "EventFlow.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventFlow.Presentation.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER app

ENTRYPOINT ["dotnet", "EventFlow.Presentation.dll"]