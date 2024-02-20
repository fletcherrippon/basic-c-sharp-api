FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish
WORKDIR /src
COPY ["BasicAPI.csproj", "./"]


RUN dotnet restore "BasicAPI.csproj" --runtime alpine-x64

COPY . .
RUN dotnet publish "BasicAPI.csproj" -c Release -o /app/publish \
    --no-restore \
    --runtime alpine-x64 \
    --self-contained true \
    /p:PublishTrimmed=true \
    /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final

RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app

RUN apk upgrade musl

USER dotnetuser
WORKDIR /app
EXPOSE 5000

COPY --from=publish /src/article.db .
COPY --from=publish /app/publish .
ENTRYPOINT ["./BasicAPI", "--urls", "http://0.0.0.0:5000"]