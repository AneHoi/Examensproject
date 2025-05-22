FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrderService/presentation/presentation.csproj", "OrderService/presentation/"]
COPY ["contracts/contracts.csproj", "contracts/"]
COPY ["OrderService/application/application.csproj", "OrderService/application/"]
COPY ["OrderService/domain/domain.csproj", "OrderService/domain/"]
COPY ["OrderService/infrastructure/infrastructure.csproj", "OrderService/infrastructure/"]
RUN dotnet restore "OrderService/presentation/presentation.csproj"
COPY . .
WORKDIR "/src/OrderService/presentation"
RUN dotnet build "presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "presentation.dll"]
