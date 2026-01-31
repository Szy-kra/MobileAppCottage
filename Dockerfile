# 1. ETAP BUDOWANIA (Używamy pełnego SDK .NET 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiujemy plik projektu i pobieramy biblioteki (NuGet)
COPY ["MobileAppCottage.csproj", "."]
RUN dotnet restore "./MobileAppCottage.csproj"

# Kopiujemy resztę kodu i budujemy wersję Release
COPY . .
RUN dotnet build "MobileAppCottage.csproj" -c Release -o /app/build

# 2. ETAP PUBLIKACJI (Przygotowanie gotowych plików do uruchomienia)
FROM build AS publish
RUN dotnet publish "MobileAppCottage.csproj" -c Release -o /app/publish

# 3. ETAP KOŃCOWY (Lekki obraz tylko do działania aplikacji)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ważne: to musi być nazwa Twojego pliku wyjściowego!
ENTRYPOINT ["dotnet", "MobileAppCottage.dll"]