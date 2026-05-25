FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY src/Wiki.Api/Wiki.Api.csproj src/Wiki.Api/
RUN dotnet restore src/Wiki.Api/Wiki.Api.csproj

COPY src/ src/
RUN dotnet publish src/Wiki.Api/Wiki.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
ENV CONTRACTS_PATH=/data/contracts

EXPOSE 5000

ENTRYPOINT ["dotnet", "Wiki.Api.dll"]
