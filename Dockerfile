# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos los csproj para restaurar dependencias
COPY ./Eyebek.sln ./
COPY ./src/Eyebek.Domain/Eyebek.Domain.csproj ./src/Eyebek.Domain/
COPY ./src/Eyebek.Application/Eyebek.Application.csproj ./src/Eyebek.Application/
COPY ./src/Eyebek.Infrastructure/Eyebek.Infrastructure.csproj ./src/Eyebek.Infrastructure/
COPY ./src/Eyebek.Api/Eyebek.Api.csproj ./src/Eyebek.Api/

# Restaurar paquetes
RUN dotnet restore ./src/Eyebek.Api/Eyebek.Api.csproj

# Copiar todo el código
COPY . .

# Publicar en modo Release
RUN dotnet publish ./src/Eyebek.Api/Eyebek.Api.csproj -c Release -o /app/publish

# Etapa final (imagen más liviana)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiamos lo publicado desde la etapa de build
COPY --from=build /app/publish .

# Render expone el puerto 8080 por defecto
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Eyebek.Api.dll"]
