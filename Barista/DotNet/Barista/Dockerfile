#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Barista.csproj", "Barista/"]
RUN dotnet restore "Barista/Barista.csproj"
WORKDIR /src/Barista
COPY . .
RUN dotnet build "Barista.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/Barista
RUN dotnet publish "Barista.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
HEALTHCHECK --interval=5s --timeout=3s CMD curl -f http://localhost:80/status || exit 1
ENTRYPOINT ["dotnet", "Barista.dll"]

