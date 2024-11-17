# Используем базовый образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проекта
COPY . .

# Восстанавливаем зависимости и собираем проект
RUN dotnet restore
RUN dotnet publish -c Release -o /app

# Используем базовый образ .NET Runtime для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем собранное приложение из стадии сборки
COPY --from=build /app .

# Копируем appsettings.json из подпапки api
COPY api/appsettings.json ./

# Указываем порт, который будет использоваться приложением
EXPOSE 8080

# Запускаем приложение
ENTRYPOINT ["dotnet", "api.dll"]