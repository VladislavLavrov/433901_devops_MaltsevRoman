# Этап 1. Базовый образ для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5008

# Этап 2. Сборка приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копирование файла проекта
COPY ["8_Calculator/8_Calculator.csproj", "8_Calculator/"]

# Восстановление зависимостей
RUN dotnet restore "8_Calculator/8_Calculator.csproj"

# Копирование остальных файлов проекта
COPY . .

# Переход в папку проекта
WORKDIR "/src/8_Calculator"

# Публикация приложения
RUN dotnet publish "8_Calculator.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап 3. Создание итогового образа
FROM base AS final
WORKDIR /app

# Копирование опубликованного приложения
COPY --from=build /app/publish .

# Настройка порта приложения
ENV ASPNETCORE_URLS=http://+:5008

# Запуск приложения
ENTRYPOINT ["dotnet", "8_Calculator.dll"]