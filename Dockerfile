# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto
COPY ["Simulacion/Simulacion.csproj", "Simulacion/"]
COPY ["Simulacion.Application/Simulacion.Application.csproj", "Simulacion.Application/"]
COPY ["Simulacion.Domain/Simulacion.Domain.csproj", "Simulacion.Domain/"]
COPY ["Simulacion.Data/Simulacion.Data.csproj", "Simulacion.Data/"]

# Restaurar dependencias
RUN dotnet restore "Simulacion/Simulacion.csproj"

# Copiar el resto del código y compilar
COPY . .
WORKDIR "/src/Simulacion"
RUN dotnet publish "Simulacion.csproj" -c Release -o /app/publish

# Etapa final de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Cloud Run escucha por defecto en el puerto 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Simulacion.dll"]