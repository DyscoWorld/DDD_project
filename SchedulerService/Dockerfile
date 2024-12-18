FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SchedulerService/SchedulerService.csproj", "SchedulerService/"]
RUN dotnet restore "SchedulerService/SchedulerService.csproj"

COPY ["SchedulerService/", "SchedulerService/"]
RUN dotnet publish "SchedulerService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SchedulerService.dll"]
