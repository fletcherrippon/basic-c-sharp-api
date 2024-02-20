FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish
WORKDIR /src
COPY ["Article.Api.csproj", "./"]


RUN dotnet restore "Article.Api.csproj" --runtime alpine-x64

COPY . .
RUN dotnet publish "Article.Api.csproj" -c Release -o /app/publish \
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
ENTRYPOINT ["./Article.Api", "--urls", "http://0.0.0.0:5000"]