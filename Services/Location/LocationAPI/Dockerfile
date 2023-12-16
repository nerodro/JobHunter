#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Location/LocationAPI/LocationAPI.csproj", "Services/Location/LocationAPI/"]
COPY ["Services/Location/LocationDomain/LocationDomain.csproj", "Services/Location/LocationDomain/"]
COPY ["Services/Location/LocationRepository/LocationRepository.csproj", "Services/Location/LocationRepository/"]
COPY ["Services/Location/LocationService/LocationService.csproj", "Services/Location/LocationService/"]
RUN dotnet restore "Services/Location/LocationAPI/LocationAPI.csproj"
COPY . .
WORKDIR "/src/Services/Location/LocationAPI"
RUN dotnet build "LocationAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocationAPI.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocationAPI.dll"]