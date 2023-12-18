#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Category/CategoryAPI/CategoryAPI.csproj", "Services/Category/CategoryAPI/"]
COPY ["Services/Category/CategoryDomain/CategoryDomain.csproj", "Services/Category/CategoryDomain/"]
COPY ["Services/Category/CategoryRepository/CategoryRepository.csproj", "Services/Category/CategoryRepository/"]
COPY ["Services/Category/CategoryService/CategoryService.csproj", "Services/Category/CategoryService/"]
RUN dotnet restore "Services/Category/CategoryAPI/CategoryAPI.csproj"
COPY . .
WORKDIR "/src/Services/Category/CategoryAPI"
RUN dotnet build "CategoryAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CategoryAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Category.API.dll"]