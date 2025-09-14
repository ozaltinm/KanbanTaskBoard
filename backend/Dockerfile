# --- build stage ---
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    COPY . .
    RUN dotnet restore
    RUN dotnet publish -c Release -o /app /p:UseAppHost=false
    
    # --- runtime stage ---
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
    WORKDIR /app
    COPY --from=build /app .
    # Konteynerin dinleyeceği port (compose ile 5121'e map edeceğiz)
    ENV ASPNETCORE_URLS=http://+:8080
    EXPOSE 8080
    ENTRYPOINT ["dotnet", "KanbanTaskBoard.Api.dll"]
    