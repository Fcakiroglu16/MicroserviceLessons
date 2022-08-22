FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ServiceC/ServiceC.csproj", "ServiceC/"]
RUN dotnet restore "ServiceC/ServiceC.csproj"
COPY . .
WORKDIR "/src/ServiceC"
RUN dotnet build "ServiceC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceC.dll"]
