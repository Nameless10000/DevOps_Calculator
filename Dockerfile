#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["3_Calculator.csproj", "."]
RUN dotnet restore "./3_Calculator.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "3_Calculator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "3_Calculator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "3_Calculator.dll"]