FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /source

COPY . .

RUN dotnet tool restore

RUN mkdir ./db
RUN touch ./db/todo.db

RUN dotnet ef database update

RUN dotnet publish -c Release -o /out

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /out .
COPY --from=build /source/db/todo.db ./db/todo.db

EXPOSE 5000

ENTRYPOINT ["./BasicAPI", "--urls", "http://0.0.0.0:5000"]