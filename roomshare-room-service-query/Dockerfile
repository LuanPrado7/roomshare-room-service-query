#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:5001;http://+:80;
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["roomshare-room-service-query/roomshare-room-service-query.csproj", "roomshare-room-service-query/"]
RUN dotnet restore "roomshare-room-service-query/roomshare-room-service-query.csproj"
COPY . .
WORKDIR "/src/roomshare-room-service-query"
RUN dotnet build "roomshare-room-service-query.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "roomshare-room-service-query.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "roomshare-room-service-query.dll"]