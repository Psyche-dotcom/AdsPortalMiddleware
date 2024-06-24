# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8085


# Use the official ASP.NET Core SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /build

# Copy the project files and restore dependencies
COPY ["AdsReportingPortal.Api/AdsReportingPortal.Api.csproj", "AdsReportingPortal.Api/"]
COPY ["AdsReportingPortal.Data/AdsReportingPortal.Data.csproj", "AdsReportingPortal.Data/"]
COPY ["AdsReportingPortal.Model/AdsReportingPortal.Model.csproj", "AdsReportingPortal.Model/"]
RUN dotnet restore "AdsReportingPortal.Api/AdsReportingPortal.Api.csproj"

# Copy the rest of the files and build the project
COPY . .
WORKDIR "/build/AdsReportingPortal.Api"
RUN dotnet build "AdsReportingPortal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AdsReportingPortal.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "AdsReportingPortal.Api.dll"]
