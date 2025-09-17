# ---- Base (ASP.NET 9) ----
    FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
    WORKDIR /app
    EXPOSE 8080
    
    # ---- Build (SDK 9) ----
    FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
    WORKDIR /src
    
    COPY ["KanbanTaskBoard.Api.csproj", "."]
    RUN dotnet restore "./KanbanTaskBoard.Api.csproj"
    
    COPY . .
    RUN dotnet build "KanbanTaskBoard.Api.csproj" -c Release -o /app/build
    
    FROM build AS publish
    RUN dotnet publish "KanbanTaskBoard.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false
    
    # ---- Final ----
    FROM base AS final
    WORKDIR /app
    RUN mkdir -p /app/data && chmod 0777 /app/data
    COPY --from=publish /app/publish .
    ENTRYPOINT ["dotnet", "KanbanTaskBoard.Api.dll"]
    