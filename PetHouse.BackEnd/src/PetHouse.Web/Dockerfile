FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PetHouse.API/PetHouse.API.csproj", "src/PetHouse.API/"]
COPY ["src/PetHouse.Application/PetHouse.Application.csproj", "src/PetHouse.Application/"]
COPY ["src/PetHouse.Domain/PetHouse.Domain.csproj", "src/PetHouse.Domain/"]
COPY ["src/PetHouse.Infrastructure/PetHouse.Infrastructure.csproj", "src/PetHouse.Infrastructure/"]
RUN dotnet restore "src/PetHouse.API/PetHouse.API.csproj"
COPY . .
WORKDIR "/src/src/PetHouse.API"
RUN dotnet build "PetHouse.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PetHouse.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetHouse.API.dll"]
