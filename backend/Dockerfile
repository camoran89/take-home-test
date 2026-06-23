FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj", "src/Fundo.Applications.WebApi/"]
RUN dotnet restore "src/Fundo.Applications.WebApi/Fundo.Applications.WebApi.csproj"
COPY src/. ./src/
WORKDIR /src/src/Fundo.Applications.WebApi
RUN dotnet publish "Fundo.Applications.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fundo.Applications.WebApi.dll"]
