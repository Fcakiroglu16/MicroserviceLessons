FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Microservice2.API/Microservice2.API.csproj", "Microservice2.API/"]
RUN dotnet restore "Microservice2.API/Microservice2.API.csproj"
COPY . .
WORKDIR "/src/Microservice2.API"
RUN dotnet build "Microservice2.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Microservice2.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservice2.API.dll"]
