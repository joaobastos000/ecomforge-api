FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY EcomForge.sln ./
COPY src/EcomForge.Api/EcomForge.Api.csproj src/EcomForge.Api/
COPY src/EcomForge.Application/EcomForge.Application.csproj src/EcomForge.Application/
COPY src/EcomForge.Common/EcomForge.Common.csproj src/EcomForge.Common/
COPY src/EcomForge.Domain/EcomForge.Domain.csproj src/EcomForge.Domain/
COPY src/EcomForge.Infrastructure/EcomForge.Infrastructure.csproj src/EcomForge.Infrastructure/
COPY src/EcomForge.Modules/EcomForge.Modules.csproj src/EcomForge.Modules/

RUN dotnet restore

COPY . .
RUN dotnet publish src/EcomForge.Api/EcomForge.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EcomForge.Api.dll"]
