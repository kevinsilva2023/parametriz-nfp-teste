# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./BackgroundServices/Parametriz.AutoNFP.ConsoleApp/Parametriz.AutoNFP.ConsoleApp.csproj", "BackgroundServices/Parametriz.AutoNFP.ConsoleApp/"]
COPY ["./Services/Parametriz.AutoNFP.Data/Parametriz.AutoNFP.Data.csproj", "Services/Parametriz.AutoNFP.Data/"]
COPY ["./Services/Parametriz.AutoNFP.Domain/Parametriz.AutoNFP.Domain.csproj", "Services/Parametriz.AutoNFP.Domain/"]
COPY ["./Common/Parametriz.AutoNFP.Core/Parametriz.AutoNFP.Core.csproj", "Common/Parametriz.AutoNFP.Core/"]
RUN dotnet restore "./BackgroundServices/Parametriz.AutoNFP.ConsoleApp/Parametriz.AutoNFP.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/BackgroundServices/Parametriz.AutoNFP.ConsoleApp"
RUN dotnet build "./Parametriz.AutoNFP.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Parametriz.AutoNFP.ConsoleApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update
RUN apt-get install -y locales locales-all
ENV LC_ALL pt_BR.UTF-8
ENV LANG pt_BR.UTF-8
ENV LANGUAGE pt_BR.UTF-8

ENTRYPOINT ["dotnet", "Parametriz.AutoNFP.ConsoleApp.dll"]