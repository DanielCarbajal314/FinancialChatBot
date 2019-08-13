FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Financial.Presentation.ChatWebServer/Financial.Presentation.ChatWebServer.csproj", "Financial.Presentation.ChatWebServer/"]
COPY ["Financial.Infrastructure.MessageQueu/Financial.Infrastructure.MessageQueu.csproj", "Financial.Infrastructure.MessageQueu/"]
COPY ["Financial.Infrastructure.ExternalServices.StooqClient/Financial.Infrastructure.ExternalServices.StooqClient.csproj", "Financial.Infrastructure.ExternalServices.StooqClient/"]
COPY ["Financial.Services.EFImplementation/Financial.Services.EFImplementation.csproj", "Financial.Services.EFImplementation/"]
COPY ["Financial.Services.Interfaces/Financial.Services.Interfaces.csproj", "Financial.Services.Interfaces/"]
COPY ["Financial.Domain.Entities/Financial.Domain.Entities.csproj", "Financial.Domain.Entities/"]
COPY ["Financial.Infrastructure.EFDataPersistance/Financial.Infrastructure.EFDataPersistance.csproj", "Financial.Infrastructure.EFDataPersistance/"]
RUN dotnet restore "Financial.Presentation.ChatWebServer/Financial.Presentation.ChatWebServer.csproj"
COPY . .
WORKDIR "/src/Financial.Presentation.ChatWebServer"
RUN dotnet build "Financial.Presentation.ChatWebServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Financial.Presentation.ChatWebServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Financial.Presentation.ChatWebServer.dll"]