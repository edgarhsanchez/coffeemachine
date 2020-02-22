#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CoffeeMachine.csproj", "CoffeeMachine/"]
RUN dotnet restore "CoffeeMachine/CoffeeMachine.csproj"
WORKDIR /src/CoffeeMachine
COPY . .
RUN dotnet build "CoffeeMachine.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/CoffeeMachine
RUN dotnet publish "CoffeeMachine.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
HEALTHCHECK --interval=5s --timeout=3s CMD curl -f http://localhost:80/status || exit 1
ENTRYPOINT ["dotnet", "CoffeeMachine.dll"]

