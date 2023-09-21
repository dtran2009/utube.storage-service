#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["StorageService.Api/StorageService.Api.csproj", "StorageService.Api/"]
COPY ["StorageService.Application/StorageService.Application.csproj", "StorageService.Application/"]
COPY ["StorageService.Infrastructure/StorageService.Infrastructure.csproj", "StorageService.Infrastructure/"]

RUN --mount=type=secret,id=github_token \
    dotnet nuget add source --username abdurrahim373 --password $(cat /run/secrets/github_token) --store-password-in-clear-text --name github "https://nuget.pkg.github.com/letslearn373/index.json"

RUN dotnet restore "StorageService.Api/StorageService.Api.csproj"
COPY . .
WORKDIR "/src/StorageService.Api"
RUN dotnet build "StorageService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StorageService.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StorageService.Api.dll"]