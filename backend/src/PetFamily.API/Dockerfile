FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/PetFamily.API/PetFamily.API.csproj", "src/PetFamily.API/"]
COPY ["src/PetFamily.Application/PetFamily.Application.csproj", "src/PetFamily.Application/"]
COPY ["src/PetFamily.Domain/PetFamily.Domain.csproj", "src/PetFamily.Domain/"]
COPY ["src/PetFamily.Infrastructure/PetFamily.Infrastructure.csproj", "src/PetFamily.Infrastructure/"]
RUN dotnet restore "src/PetFamily.API/PetFamily.API.csproj"
COPY . .
WORKDIR "/src/src/PetFamily.API"
RUN dotnet build "PetFamily.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PetFamily.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PetFamily.API.dll"]
