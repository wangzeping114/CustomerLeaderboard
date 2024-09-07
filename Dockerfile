FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
ENV TZ=Asia/Shanghai
ARG ASPNETCORE_ENVIRONMENT
ENV ASPNETCORE_ENVIRONMENT=Development

# 复制 nuget.config 文件到镜像中
COPY nuget.config .

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

WORKDIR "/src/src"
COPY --from=base /app/nuget.config .

RUN dotnet build "CustomerLeaderboard.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerLeaderboard.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .    
ENTRYPOINT ["dotnet", "CustomerLeaderboard.Api.dll"]

