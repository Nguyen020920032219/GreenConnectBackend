# Stage 1: Build (Giữ nguyên như cũ vì đã tốt rồi)
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

# Stage 2: Runtime (Đã thêm cài đặt giờ VN)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# --- BẮT ĐẦU PHẦN THÊM MỚI ---
# Cài đặt Timezone sang Asia/Ho_Chi_Minh (GMT+7)
RUN apt-get update && apt-get install -y tzdata
ENV TZ=Asia/Ho_Chi_Minh
# --- KẾT THÚC PHẦN THÊM MỚI ---

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "GreenConnectPlatform.Api.dll"]