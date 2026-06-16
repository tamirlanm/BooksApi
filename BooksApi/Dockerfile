#stage 1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["BooksApi.csproj", "."]
RUN dotnet restore "BooksApi.csproj"
COPY . .
RUN dotnet build -c Release -o /app/build

#stage 2
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

#stage 3
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "BooksApi.dll" ]