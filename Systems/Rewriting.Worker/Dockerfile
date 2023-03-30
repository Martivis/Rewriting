#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Rewriting.Worker/Rewriting.Worker.csproj", "Systems/Rewriting.Worker/"]
COPY ["Services/Rewriting.Services.RabbitMQ/Rewriting.Services.RabbitMQ.csproj", "Services/Rewriting.Services.RabbitMQ/"]
COPY ["Shared/Rewriting.SettingsLoader/Rewriting.Settings.csproj", "Shared/Rewriting.SettingsLoader/"]
COPY ["Services/Rewriting.Services.SmtpSender/Rewriting.Services.SmtpSender.csproj", "Services/Rewriting.Services.SmtpSender/"]
RUN dotnet restore "Systems/Rewriting.Worker/Rewriting.Worker.csproj"
COPY . .
WORKDIR "/src/Systems/Rewriting.Worker"
RUN dotnet build "Rewriting.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rewriting.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rewriting.Worker.dll"]