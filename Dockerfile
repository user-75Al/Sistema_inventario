# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /app

# Copiar archivos del proyecto y restaurar dependencias
COPY src/*.csproj ./src/
RUN dotnet restore src/UtmMarket.csproj

# Copiar el resto del código y compilar
COPY . ./
RUN dotnet publish src/UtmMarket.csproj -c Release -o out

# Etapa final (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /app/out .

# Exponer el puerto que usa Render
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "UtmMarket.dll"]
