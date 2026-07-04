FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ProjetoTeste/ProjetoTeste.csproj ProjetoTeste/
RUN dotnet restore ProjetoTeste/ProjetoTeste.csproj

COPY ProjetoTeste/ ProjetoTeste/
WORKDIR /app/ProjetoTeste
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/ProjetoTeste/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "ProjetoTeste.dll"]
