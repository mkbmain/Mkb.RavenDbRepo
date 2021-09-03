FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["RavenDb/RavenDb.csproj", "RavenDb/"]
RUN dotnet restore "RavenDb/RavenDb.csproj"
COPY . .
WORKDIR "/src/RavenDb"
RUN dotnet build "RavenDb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RavenDb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RavenDb.dll"]
