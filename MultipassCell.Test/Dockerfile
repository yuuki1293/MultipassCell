FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MultipassCell.Test/MultipassCell.Test.fsproj", "MultipassCell.Test/"]
RUN dotnet restore "MultipassCell.Test/MultipassCell.Test.fsproj"
COPY . .
WORKDIR "/src/MultipassCell.Test"
RUN dotnet build "MultipassCell.Test.fsproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MultipassCell.Test.fsproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MultipassCell.Test.dll"]
