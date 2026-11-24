
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5008

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["8_Сalculator/8_Сalculator.csproj", "8_Сalculator/"]
RUN dotnet restore "8_Сalculator/8_Сalculator.csproj"
COPY . .
WORKDIR "/src/8_Сalculator"
RUN dotnet publish "8_Сalculator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:5008
ENTRYPOINT ["dotnet", "8_Сalculator.dll"]
