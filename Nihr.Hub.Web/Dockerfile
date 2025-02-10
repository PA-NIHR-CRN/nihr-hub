FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG NUGET_PACKAGE_USERNAME
ARG NUGET_PACKAGE_TOKEN
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Nihr.Hub.Web/Nihr.Hub.Web.csproj", "Nihr.Hub.Web/"]
COPY ["Nihr.Hub.Infrastructure/Nihr.Hub.Infrastructure.csproj", "Nihr.Hub.Infrastructure/"]
COPY ["Nihr.Hub.Domain/Nihr.Hub.Domain.csproj", "Nihr.Hub.Domain/"]
RUN dotnet nuget add source --username $NUGET_PACKAGE_USERNAME --password $NUGET_PACKAGE_TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/pa-nihr-crn/index.json"
RUN dotnet restore "Nihr.Hub.Web/Nihr.Hub.Web.csproj"
COPY . .
WORKDIR "/src/Nihr.Hub.Web"
RUN dotnet build "Nihr.Hub.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Nihr.Hub.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nihr.Hub.Web.dll"]
