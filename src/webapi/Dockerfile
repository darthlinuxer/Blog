FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything
COPY . .
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish src/webapi -c Release -o /App/out 

# Build runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "webapi.dll"]