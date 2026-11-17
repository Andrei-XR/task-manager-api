# Etapa 1: compilar e publicar usando o SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia só o .csproj e restaura os pacotes (aproveita o cache do Docker)
COPY TaskManager.Api/TaskManager.Api.csproj TaskManager.Api/
RUN dotnet restore TaskManager.Api/TaskManager.Api.csproj

# Copia o restante do código e publica
COPY TaskManager.Api/ TaskManager.Api/
RUN dotnet publish TaskManager.Api/TaskManager.Api.csproj -c Release -o /app/publish --no-restore

# Etapa 2: imagem final, só com o runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]