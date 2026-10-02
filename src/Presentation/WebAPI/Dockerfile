# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Portfolio.sln", "./"]
COPY ["src/Core/Domain/Portfolio.Domain.csproj", "src/Core/Domain/"]
COPY ["src/Core/Application/Portfolio.Application.csproj", "src/Core/Application/"]
COPY ["src/Infrastructure/Persistence/Portfolio.Infrastructure.Persistence.csproj", "src/Infrastructure/Persistence/"]
COPY ["src/Presentation/WebAPI/Portfolio.Presentation.WebAPI.csproj", "src/Presentation/WebAPI/"]

RUN dotnet restore "src/Presentation/WebAPI/Portfolio.Presentation.WebAPI.csproj"

# Copy source code and build
COPY . .
WORKDIR "/src/src/Presentation/WebAPI"
RUN dotnet publish "Portfolio.Presentation.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy published output
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Set environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Portfolio.Presentation.WebAPI.dll"]