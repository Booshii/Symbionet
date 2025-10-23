# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# NuGet Restore separat ausführen
COPY MaiVe.csproj ./
RUN dotnet restore

# Kopiere den restlichen Code & baue die Anwendung
COPY . ./
RUN dotnet build -c Release --no-restore
RUN dotnet publish -c Release -o /out --no-restore

# Stage 2: Runtime Image 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

# Port setzen
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

# Start der App 
ENTRYPOINT ["dotnet", "MaiVe.dll"]
