#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/User/User.API/User.API.csproj", "Services/User/User.API/"]
COPY ["Services/User/UserDomain/UserDomain.csproj", "Services/User/UserDomain/"]
COPY ["Services/User/UserRepository/UserRepository.csproj", "Services/User/UserRepository/"]
COPY ["Services/User/UserService/UserService.csproj", "Services/User/UserService/"]
RUN dotnet restore "Services/User/UserAPI/UserAPI.csproj"
COPY . .
WORKDIR "/src/Services/User/UserAPI"
RUN dotnet build "UserAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserAPI.dll"]