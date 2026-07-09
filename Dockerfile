# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore first (better layer caching)
COPY KanbanApp.csproj ./
RUN dotnet restore KanbanApp.csproj

# Copy the rest and publish
COPY . ./
RUN dotnet publish KanbanApp.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Render routes traffic to the port in $PORT (default 10000).
# Kestrel listens on that port; 8080 is the fallback for local `docker run`.
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "KanbanApp.dll"]
