#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["YukiBlog.WP/YukiBlog.WP.fsproj", "YukiBlog.WP/"]
COPY ["YukiBlog.WP.Themes/YukiBlog.WP.Themes.msbuildproj", "YukiBlog.WP.Themes/"]
COPY ["YukiBlog.WP.Plugins/YukiBlog.WP.Plugins.msbuildproj", "YukiBlog.WP.Plugins/"]
RUN dotnet restore "YukiBlog.WP/YukiBlog.WP.fsproj"
COPY . .
WORKDIR "/src/YukiBlog.WP"
RUN dotnet build "YukiBlog.WP.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YukiBlog.WP.fsproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YukiBlog.WP.dll"]