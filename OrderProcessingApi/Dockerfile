FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["OrderProcessingApi/OrderProcessingApi.csproj", "OrderProcessingApi/"]
COPY ["OrderProcessing.Application/OrderProcessing.Application.csproj", "OrderProcessing.Application/"]
COPY ["OrderProcessing.Domain/OrderProcessing.Domain.csproj", "OrderProcessing.Domain/"]
COPY ["OrderProcessing.Infrastructure/OrderProcessing.Infrastructure.csproj", "OrderProcessing.Infrastructure/"]

RUN dotnet restore "OrderProcessingApi/OrderProcessingApi.csproj"

COPY . .

WORKDIR "/src/OrderProcessingApi"

RUN dotnet publish "OrderProcessingApi.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrderProcessingApi.dll"]