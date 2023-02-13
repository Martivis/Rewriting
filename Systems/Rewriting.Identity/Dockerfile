#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Rewriting.Identity/Rewriting.Identity.csproj", "Systems/Rewriting.Identity/"]
COPY ["Data/Rewriting.Context.Entities/Rewriting.Context.Entities.csproj", "Data/Rewriting.Context.Entities/"]
COPY ["Data/Rewriting.Context.Migrations/Rewriting.Context.Migrations.csproj", "Data/Rewriting.Context.Migrations/"]
COPY ["Data/Rewriting.Context/Rewriting.Context.csproj", "Data/Rewriting.Context/"]
COPY ["Shared/Rewriting.SettingsLoader/Rewriting.Settings.csproj", "Shared/Rewriting.SettingsLoader/"]
COPY ["Shared/Rewriting.Common/Rewriting.Common.csproj", "Shared/Rewriting.Common/"]
RUN dotnet restore "Systems/Rewriting.Identity/Rewriting.Identity.csproj"
COPY . .
WORKDIR "/src/Systems/Rewriting.Identity"
RUN dotnet build "Rewriting.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rewriting.Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rewriting.Identity.dll"]