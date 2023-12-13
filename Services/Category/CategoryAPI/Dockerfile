#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Category/CategoryAPI/CategoryAPI.csproj", "CategoryAPI/"]
COPY ["Services/Category/CategoryDomain/CategoryDomain.csproj", "CategoryDomain/"]
COPY ["Services/Category/CategoryRepository/CategoryRepository.csproj", "CategoryRepository/"]
COPY ["Services/Category/CategoryService/CategoryService.csproj", "CategoryService/"]
RUN dotnet restore "CategoryAPI/CategoryAPI.csproj"
COPY . .
WORKDIR "/src/CategoryAPI"
RUN dotnet build "CategoryAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CategoryAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CategoryAPI.dll"]