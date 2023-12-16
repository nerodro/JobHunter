#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Company/Company.API/Company.API.csproj", "Services/Company/Company.API/"]
COPY ["Services/Company/CompanyDomain/CompanyDomain.csproj", "Services/Company/CompanyDomain/"]
COPY ["Services/Company/CompanyRepository/CompanyRepository.csproj", "Services/Company/CompanyRepository/"]
COPY ["Services/Company/CompanyService/CompanyService.csproj", "Services/Company/CompanyService/"]
RUN dotnet restore "Services/Company/CompanyAPI/CompanyAPI.csproj"
COPY . .
WORKDIR "/src/Services/Company/CompanyAPI"
RUN dotnet build "CompanyAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CompanyAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompanyAPI.dll"]