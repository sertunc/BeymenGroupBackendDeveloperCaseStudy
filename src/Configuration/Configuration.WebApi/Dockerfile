FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Configuration/Configuration.WebApi/Configuration.WebApi.csproj", "src/Configuration/Configuration.WebApi/"]
COPY ["src/Configuration/Configuration.Business/Configuration.Business.csproj", "src/Configuration/Configuration.Business/"]
COPY ["src/Configuration/Configuration.Business.Abstractions/Configuration.Business.Abstractions.csproj", "src/Configuration/Configuration.Business.Abstractions/"]
COPY ["src/Shared/BeymenGroup.Shared/BeymenGroup.Shared.csproj", "src/Shared/BeymenGroup.Shared/"]
COPY ["src/Configuration/Configuration.Common/Configuration.Common.csproj", "src/Configuration/Configuration.Common/"]
COPY ["src/Configuration/Configuration.Data.Abstractions/Configuration.Data.Abstractions.csproj", "src/Configuration/Configuration.Data.Abstractions/"]
COPY ["src/Configuration/Configuration.Data.Models/Configuration.Data.Models.csproj", "src/Configuration/Configuration.Data.Models/"]
COPY ["src/Configuration/Configuration.Data.MongoDB/Configuration.Data.MongoDB.csproj", "src/Configuration/Configuration.Data.MongoDB/"]
RUN dotnet restore "./src/Configuration/Configuration.WebApi/Configuration.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Configuration/Configuration.WebApi"
RUN dotnet build "./Configuration.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Configuration.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Configuration.WebApi.dll"]