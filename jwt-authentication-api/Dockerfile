FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY WebApi.sln .
COPY WebApi/WebApi.csproj ./src/
RUN dotnet restore

# copy everything else and build app
COPY WebApi/. ./src/
WORKDIR /app/src
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/src/out ./
ENTRYPOINT ["dotnet", "WebApi.dll"]
