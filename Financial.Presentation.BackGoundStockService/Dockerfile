FROM mcr.microsoft.com/dotnet/core/runtime:2.1-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Financial.Presentation.BackGoundStockService/Financial.Presentation.BackGoundStockService.csproj", "Financial.Presentation.BackGoundStockService/"]
COPY ["Financial.Infrastructure.MessageQueu/Financial.Infrastructure.MessageQueu.csproj", "Financial.Infrastructure.MessageQueu/"]
COPY ["Financial.Infrastructure.ExternalServices.StooqClient/Financial.Infrastructure.ExternalServices.StooqClient.csproj", "Financial.Infrastructure.ExternalServices.StooqClient/"]
RUN dotnet restore "Financial.Presentation.BackGoundStockService/Financial.Presentation.BackGoundStockService.csproj"
COPY . .
WORKDIR "/src/Financial.Presentation.BackGoundStockService"
RUN dotnet build "Financial.Presentation.BackGoundStockService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Financial.Presentation.BackGoundStockService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Financial.Presentation.BackGoundStockService.dll"]
