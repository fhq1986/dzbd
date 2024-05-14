#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5005

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep ./Admin.NET/Admin.NET.Web.Entry/adminet.pfx  -p crypticpassword
COPY ["./Admin.NET/Admin.NET.Web.Entry/*.*", "Admin.NET.Web.Entry/"]
COPY ["./Admin.NET/Admin.NET.Web.Core/Admin.NET.Web.Core.csproj", "Admin.NET.Web.Core/"]
COPY ["./Admin.NET/Admin.NET.Application/Admin.NET.Application.csproj", "Admin.NET.Application/"]
COPY ["./Admin.NET/Admin.NET.Application/Configuration/*.*", "Admin.NET.Application/Configuration/"]
COPY ["./Admin.NET/Admin.NET.Core/Admin.NET.Core.csproj", "Admin.NET.Core/"]
COPY ["./Admin.NET/DapperExtensions/DapperExtensions.csproj", "DapperExtensions/"]
RUN dotnet restore "./Admin.NET.Web.Entry/./Admin.NET.Web.Entry.csproj"
COPY . .
WORKDIR "/src/Admin.NET.Web.Entry"
RUN dotnet build "./Admin.NET.Web.Entry.csproj" -c $BUILD_CONFIGURATION -o /app/build -r linux-x64

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Admin.NET.Web.Entry.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Admin.NET.Web.Entry.dll"]