FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY . .
EXPOSE 6000
ENTRYPOINT ["dotnet", "Treatment.Monitor.Api.dll", "--urls=http://0.0.0.0:6000/"]