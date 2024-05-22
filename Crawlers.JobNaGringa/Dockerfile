FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Crawlers.JobNaGringa/Crawlers.JobNaGringa.csproj", "Crawlers.JobNaGringa/"]
COPY ["Crawlers.Core/Crawlers.Core.csproj", "Crawlers.Core/"]
RUN dotnet restore "Crawlers.JobNaGringa/Crawlers.JobNaGringa.csproj"
COPY . .
WORKDIR "/src/Crawlers.JobNaGringa"
RUN dotnet build "Crawlers.JobNaGringa.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Crawlers.JobNaGringa.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Crawlers.JobNaGringa.dll"]
