# Stage 1: Build resources
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SubscripSync.API/SubscripSync.API.csproj", "SubscripSync.API/"]
COPY ["SubscripSync.Application/SubscripSync.Application.csproj", "SubscripSync.Application/"]
COPY ["SubscripSync.Domain/SubscripSync.Domain.csproj", "SubscripSync.Domain/"]
COPY ["SubscripSync.Infrastructure/SubscripSync.Infrastructure.csproj", "SubscripSync.Infrastructure/"]
RUN dotnet restore "SubscripSync.API/SubscripSync.API.csproj"
COPY . .
WORKDIR "/src/SubscripSync.API"
RUN dotnet build "SubscripSync.API.csproj" -c Release -o /app/build
RUN dotnet publish "SubscripSync.API.csproj" -c Release -o /app/publish

# Stage 2: Run the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SubscripSync.API.dll"]
