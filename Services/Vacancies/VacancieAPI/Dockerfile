#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Vacancies/VacancieAPI/VacancieAPI.csproj", "Services/Vacancies/VacancieAPI/"]
COPY ["Services/Vacancies/VacancieDomain/VacancieDomain.csproj", "Services/Vacancie/VacancieDomain/"]
COPY ["Services/Vacancies/VacancieRepository/VacancieRepository.csproj", "Services/Vacancie/VacancieRepository/"]
COPY ["Services/Vacancies/VacancieService/VacancieService.csproj", "Services/Vacancie/VacancieService/"]
RUN dotnet restore "Services/Vacancies/VacancieAPI/VacancieAPI.csproj"
COPY . .
WORKDIR "/src/Services/Vacancies/VacancieAPI"
RUN dotnet build "VacancieAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VacancieAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VacancieAPI.dll"]