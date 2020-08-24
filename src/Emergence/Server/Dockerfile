#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Emergence/Server/Emergence.Client.Server.csproj", "Emergence/Server/"]
COPY ["Emergence/Client/Emergence.Client.csproj", "Emergence/Client/"]
COPY ["Emergence.Data.Shared/Emergence.Data.Shared.csproj", "Emergence.Data.Shared/"]
COPY ["Emergence.Service/Emergence.Service.csproj", "Emergence.Service/"]
COPY ["Emergence.Data/Emergence.Data.csproj", "Emergence.Data/"]
COPY ["Emergence.API/Emergence.API.csproj", "Emergence.API/"]
COPY ["Emergence.Transform/Emergence.Transform.csproj", "Emergence.Transform/"]
COPY ["Emergence.Data.External/Emergence.Data.External.csproj", "Emergence.Data.External/"]
RUN dotnet restore "Emergence/Server/Emergence.Client.Server.csproj"
COPY . .
WORKDIR "/src/Emergence/Server"
RUN dotnet build "Emergence.Client.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Emergence.Client.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Emergence.Client.Server.dll"]