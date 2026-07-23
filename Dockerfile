# --- build ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files first for restore layer caching.
COPY src/ExecMagica.Domain/ExecMagica.Domain.csproj src/ExecMagica.Domain/
COPY src/ExecMagica.Application/ExecMagica.Application.csproj src/ExecMagica.Application/
COPY src/ExecMagica.Infrastructure/ExecMagica.Infrastructure.csproj src/ExecMagica.Infrastructure/
COPY src/ExecMagica.Api/ExecMagica.Api.csproj src/ExecMagica.Api/
RUN dotnet restore src/ExecMagica.Api/ExecMagica.Api.csproj

# Copy the rest and publish.
COPY . .
RUN dotnet publish src/ExecMagica.Api/ExecMagica.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# --- runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "ExecMagica.Api.dll"]