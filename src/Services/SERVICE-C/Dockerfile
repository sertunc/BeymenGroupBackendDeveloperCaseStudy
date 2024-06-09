FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/SERVICE-C/SERVICE-C.csproj", "src/Services/SERVICE-C/"]
COPY ["src/Configuration/Configuration.Data.MongoDB/Configuration.Data.MongoDB.csproj", "src/Configuration/Configuration.Data.MongoDB/"]
COPY ["src/Configuration/Configuration.Common/Configuration.Common.csproj", "src/Configuration/Configuration.Common/"]
COPY ["src/Configuration/Configuration.Data.Abstractions/Configuration.Data.Abstractions.csproj", "src/Configuration/Configuration.Data.Abstractions/"]
COPY ["src/Configuration/Configuration.Data.Models/Configuration.Data.Models.csproj", "src/Configuration/Configuration.Data.Models/"]
COPY ["src/Configuration/Configuration.Library/Configuration.Library.csproj", "src/Configuration/Configuration.Library/"]
RUN dotnet restore "./src/Services/SERVICE-C/SERVICE-C.csproj"
COPY . .
WORKDIR "/src/src/Services/SERVICE-C"
RUN dotnet build "./SERVICE-C.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SERVICE-C.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SERVICE-C.dll"]