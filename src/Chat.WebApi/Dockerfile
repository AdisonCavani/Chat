#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Chat.WebApi/Chat.WebApi.csproj", "Chat.WebApi/"]
RUN dotnet restore "Chat.WebApi/Chat.WebApi.csproj"
COPY . .
WORKDIR "/src/Chat.WebApi"
RUN dotnet build "Chat.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Chat.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.WebApi.dll"]