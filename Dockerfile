# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY GreenConnectPlatformBackend.sln ./
COPY GreenConnectPlatform.Api/GreenConnectPlatform.Api.csproj ./GreenConnectPlatform.Api/
COPY GreenConnectPlatform.Business/GreenConnectPlatform.Business.csproj ./GreenConnectPlatform.Business/
COPY GreenConnectPlatform.Data/GreenConnectPlatform.Data.csproj ./GreenConnectPlatform.Data/

COPY GreenConnectPlatform.Tests/GreenConnectPlatform.Tests.csproj ./GreenConnectPlatform.Tests/
RUN dotnet restore "GreenConnectPlatformBackend.sln"

COPY . .
WORKDIR /src
RUN dotnet publish "GreenConnectPlatform.Api/GreenConnectPlatform.Api.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GreenConnectPlatform.Api.dll"]