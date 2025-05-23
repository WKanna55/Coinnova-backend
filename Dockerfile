# PARA RENDER NO PONER .ENV ni variables de entorno, esto en la plataforma
# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar el archivo de solución y los proyectos
COPY Coinnova.API.sln ./
COPY Coinnova.API/Coinnova.API.csproj Coinnova.API/
COPY Coinnova.Application/Coinnova.Application.csproj Coinnova.Application/
COPY Coinnova.Domain/Coinnova.Domain.csproj Coinnova.Domain/
COPY Coinnova.Infrastructure/Coinnova.Infrastructure.csproj Coinnova.Infrastructure/

# Restaurar dependencias
RUN dotnet restore

# Copiar el resto del código
COPY . .

# Compilar y publicar en modo Release
RUN dotnet publish Coinnova.API/Coinnova.API.csproj -c Release -o /app/out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Variables de entorno (Render las sobreescribe)
ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_ENVIRONMENT=Production

# Healthcheck para asegurar que el contenedor está activo
HEALTHCHECK --interval=30s --timeout=3s --retries=3 \
  CMD curl --fail http://localhost:5000/health || exit 1

# Exponer el puerto
EXPOSE 5000

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Coinnova.API.dll"]