#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Rewriting.API/Rewriting.API.csproj", "Systems/Rewriting.API/"]
COPY ["Data/Rewriting.Context.Entities/Rewriting.Context.Entities.csproj", "Data/Rewriting.Context.Entities/"]
COPY ["Data/Rewriting.Context.Migrations/Rewriting.Context.Migrations.csproj", "Data/Rewriting.Context.Migrations/"]
COPY ["Data/Rewriting.Context/Rewriting.Context.csproj", "Data/Rewriting.Context/"]
COPY ["Services/Rewriting.Services.UserAccount/Rewriting.Services.UserAccount.csproj", "Services/Rewriting.Services.UserAccount/"]
COPY ["Shared/Rewriting.Common/Rewriting.Common.csproj", "Shared/Rewriting.Common/"]
COPY ["Shared/Rewriting.SettingsLoader/Rewriting.Settings.csproj", "Shared/Rewriting.SettingsLoader/"]
COPY ["Services/Rewriting.Services.Contracts/Rewriting.Services.Contracts.csproj", "Services/Rewriting.Services.Contracts/"]
COPY ["Services/Rewriting.Services.Offers/Rewriting.Services.Offers.csproj", "Services/Rewriting.Services.Offers/"]
COPY ["Services/Rewriting.Services.Orders/Rewriting.Services.Orders.csproj", "Services/Rewriting.Services.Orders/"]
COPY ["Services/Rewriting.Services.Email/Rewriting.Services.Email.csproj", "Services/Rewriting.Services.Email/"]
COPY ["Services/Rewriting.Services.Notifications/Rewriting.Services.Notifications.csproj", "Services/Rewriting.Services.Notifications/"]
COPY ["Services/Rewriting.Services.RabbitMQ/Rewriting.Services.RabbitMQ.csproj", "Services/Rewriting.Services.RabbitMQ/"]

RUN dotnet restore "Systems/Rewriting.API/Rewriting.API.csproj"
COPY . .
WORKDIR "/src/Systems/Rewriting.API"
RUN dotnet build "Rewriting.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rewriting.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rewriting.API.dll"]