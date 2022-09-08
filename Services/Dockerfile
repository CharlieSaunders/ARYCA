#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Services.csproj", "Services/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Services/Services.csproj"
COPY . .
WORKDIR "/src/Services"
RUN dotnet build "Services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Services/ARYCA.db . .
ENTRYPOINT ["dotnet", "Services.dll"]