#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebTasks.Server/WebTasks.Server.csproj", "WebTasks.Server/"]
RUN dotnet restore "WebTasks.Server/WebTasks.Server.csproj"
COPY . .
WORKDIR "/src/WebTasks.Server"
RUN dotnet build "WebTasks.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebTasks.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebTasks.Server.dll"]