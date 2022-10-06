FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WorkerService2/WorkerService2.csproj", "WorkerService2/"]
RUN dotnet restore "WorkerService2/WorkerService2.csproj"
COPY . .
WORKDIR "/src/WorkerService2"
RUN dotnet build "WorkerService2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerService2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerService2.dll"]
