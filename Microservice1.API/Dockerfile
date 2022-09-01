FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Microservice1.API/Microservice1.API.csproj", "Microservice1.API/"]
RUN dotnet restore "Microservice1.API/Microservice1.API.csproj"
COPY . .
WORKDIR "/src/Microservice1.API"
RUN dotnet build "Microservice1.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservice1.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice1.API.dll"]
