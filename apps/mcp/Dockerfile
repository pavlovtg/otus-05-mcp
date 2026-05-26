FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY src/Mcp.Api/Mcp.Api.csproj src/Mcp.Api/
RUN dotnet restore src/Mcp.Api/Mcp.Api.csproj

COPY src/ src/
RUN dotnet publish src/Mcp.Api/Mcp.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5200
ENV WIKI_API_URL=http://wiki-api:5000

EXPOSE 5200

ENTRYPOINT ["dotnet", "Mcp.Api.dll"]
