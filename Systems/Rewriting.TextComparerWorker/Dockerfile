FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Systems/Rewriting.TextComparerWorker/Rewriting.TextComparerWorker.csproj", "Systems/Rewriting.TextComparerWorker/"]
COPY ["Data/Rewriting.Context.Migrations/Rewriting.Context.Migrations.csproj", "Rewriting.Context.Migrations/"]
COPY ["Services/Rewriting.Services.Contracts/Rewriting.Services.Contracts.csproj", "Rewriting.Services.Contracts/"]
COPY ["Data/Rewriting.Context.Entities/Rewriting.Context.Entities.csproj", "Rewriting.Context.Entities/"]
COPY ["Data/Rewriting.Context/Rewriting.Context.csproj", "Rewriting.Context/"]
COPY ["Services/Rewriting.Services.UserAccount/Rewriting.Services.UserAccount.csproj", "Rewriting.Services.UserAccount/"]
COPY ["Shared/Rewriting.Common/Rewriting.Common.csproj", "Rewriting.Common/"]
COPY ["Services/Rewriting.Services.Email/Rewriting.Services.Email.csproj", "Rewriting.Services.Email/"]
COPY ["Services/Rewriting.Services.RabbitMQ/Rewriting.Services.RabbitMQ.csproj", "Rewriting.Services.RabbitMQ/"]
COPY ["Shared/Rewriting.SettingsLoader/Rewriting.Settings.csproj", "Rewriting.SettingsLoader/"]
COPY ["Services/Rewriting.Services.SmtpSender/Rewriting.Services.SmtpSender.csproj", "Rewriting.Services.SmtpSender/"]
COPY ["Services/Rewriting.Services.Cache/Rewriting.Services.Cache.csproj", "Rewriting.Services.Cache/"]
COPY ["Services/Rewriting.Services.TextComparer/Rewriting.Services.TextComparer.csproj", "Rewriting.Services.TextComparer/"]
RUN dotnet restore "Systems/Rewriting.TextComparerWorker/Rewriting.TextComparerWorker.csproj"
COPY . .
WORKDIR "/src/Systems/Rewriting.TextComparerWorker"
RUN dotnet build "Rewriting.TextComparerWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rewriting.TextComparerWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rewriting.TextComparerWorker.dll"]
