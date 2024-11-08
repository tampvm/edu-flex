# Use aspnet runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Set the environment to Development so the application uses appsettings.Development.json
ENV ASPNETCORE_ENVIRONMENT=Development

# Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EduFlex.API/EduFlex.API.csproj", "EduFlex.API/"]
COPY ["EduFlex.Service/EduFlex.Service.csproj", "EduFlex.Service/"]
COPY ["EduFlex.Repository/EduFlex.Repository.csproj", "EduFlex.Repository/"]
COPY ["EduFlex.Domain/EduFlex.Domain.csproj", "EduFlex.Domain/"]
RUN dotnet restore "./EduFlex.API/EduFlex.API.csproj"
COPY . .
WORKDIR "/src/EduFlex.API"
RUN dotnet build "./EduFlex.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EduFlex.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: setup runtime environment
FROM base AS final
WORKDIR /app

# Copy the publish folder
COPY --from=publish /app/publish .

# Copy appsettings.Development.json to the container
COPY ["EduFlex.API/appsettings.Development.json", "./"]

# Start the application
ENTRYPOINT ["dotnet", "EduFlex.API.dll"]