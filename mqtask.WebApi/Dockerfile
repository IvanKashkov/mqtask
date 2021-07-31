#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["mqtask.WebApi/mqtask.WebApi.csproj", "mqtask.WebApi/"]
RUN dotnet restore "mqtask.WebApi/mqtask.WebApi.csproj"
COPY . .
WORKDIR "/src/mqtask.WebApi"
RUN dotnet build "mqtask.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mqtask.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mqtask.WebApi.dll"]