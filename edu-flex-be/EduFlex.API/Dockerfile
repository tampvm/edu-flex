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

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EduFlex.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduFlex.API.dll"]