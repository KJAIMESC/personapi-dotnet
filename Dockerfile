# Use the official ASP.NET Core runtime image for .NET 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET SDK image for .NET 8.0 to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore any dependencies
COPY ["personapi-dotnet/personapi-dotnet.csproj", "./"]
RUN dotnet restore "./personapi-dotnet.csproj"

# Copy the remaining source code and build the application
COPY personapi-dotnet/. ./
WORKDIR "/src"
RUN dotnet build "personapi-dotnet.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "personapi-dotnet.csproj" -c Release -o /app/publish

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "personapi-dotnet.dll"]
