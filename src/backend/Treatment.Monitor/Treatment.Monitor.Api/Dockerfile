FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Treatment.Monitor/Treatment.Monitor.csproj", "Treatment.Monitor/"]
RUN dotnet restore "Treatment.Monitor/Treatment.Monitor.csproj"
COPY . .
WORKDIR "/src/Treatment.Monitor"
RUN dotnet build "Treatment.Monitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Treatment.Monitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Treatment.Monitor.dll"]
