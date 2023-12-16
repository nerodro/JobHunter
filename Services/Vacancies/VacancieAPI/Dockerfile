#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Vacancie/Vacancie.API/Vacancie.API.csproj", "Services/Vacancie/Vacancie.API/"]
COPY ["Services/Vacancie/VacancieDomain/VacancieDomain.csproj", "Services/Vacancie/VacancieDomain/"]
COPY ["Services/Vacancie/VacancieRepository/VacancieRepository.csproj", "Services/Vacancie/VacancieRepository/"]
COPY ["Services/Vacancie/VacancieService/VacancieService.csproj", "Services/Vacancie/VacancieService/"]
RUN dotnet restore "Services/Vacancie/VacancieAPI/VacancieAPI.csproj"
COPY . .
WORKDIR "/src/Services/Vacancie/VacancieAPI"
RUN dotnet build "VacancieAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VacancieAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VacancieAPI.dll"]