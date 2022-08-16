FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExampleApp.API/ExampleApp.API.csproj", "ExampleApp.API/"]
RUN dotnet restore "ExampleApp.API/ExampleApp.API.csproj"
COPY . .
WORKDIR "/src/ExampleApp.API"
RUN dotnet build "ExampleApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExampleApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS="http://*:1453"
ENTRYPOINT ["dotnet", "ExampleApp.API.dll"]
