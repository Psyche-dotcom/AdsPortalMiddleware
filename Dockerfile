# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-focal AS build
WORKDIR /source

# Copy the project files
COPY . .

# Restore NuGet packages
RUN dotnet restore "AdsReportingPortal.Api/AdsReportingPortal.Api.csproj"





# Publish the project
RUN dotnet publish "AdsReportingPortal.Api/AdsReportingPortal.Api.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-focal AS serve
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "AdsReportingPortal.Api.dll"]
