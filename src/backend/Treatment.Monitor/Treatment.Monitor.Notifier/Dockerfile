FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Treatment.Monitor.Notifier/Treatment.Monitor.Notifier.csproj", "Treatment.Monitor.Notifier/"]
RUN dotnet restore "Treatment.Monitor.Notifier/Treatment.Monitor.Notifier.csproj"
COPY . .
WORKDIR "/src/Treatment.Monitor.Notifier"
RUN dotnet build "Treatment.Monitor.Notifier.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Treatment.Monitor.Notifier.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Treatment.Monitor.Notifier.dll"]
