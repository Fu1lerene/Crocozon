FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Crocozon.Services.ItemsData/src/Crocozon.Services.ItemsData.Host/Crocozon.Services.ItemsData.Host.csproj", "Crocozon.Services.ItemsData/src/Crocozon.Services.ItemsData.Host/"]
COPY ["Directory.Build.props", "./"]
COPY ["Directory.Packages.props", "./"]

RUN dotnet restore "Crocozon.Services.ItemsData/src/Crocozon.Services.ItemsData.Host/Crocozon.Services.ItemsData.Host.csproj"

COPY . .

WORKDIR "/src/Crocozon.Services.ItemsData/src/Crocozon.Services.ItemsData.Host"
RUN dotnet build "Crocozon.Services.ItemsData.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Crocozon.Services.ItemsData.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crocozon.Services.ItemsData.Host.dll"]