# Step 1: Use the official .NET runtime image for the final application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Step 2: Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BlazorApp/BlazorApp.csproj", "BlazorApp/"]
RUN dotnet restore "BlazorApp/BlazorApp.csproj"
COPY . .
WORKDIR "/src/BlazorApp"
RUN dotnet build "BlazorApp.csproj" -c Release -o /app/build

# Step 3: Publish the app
FROM build AS publish
RUN dotnet publish "BlazorApp.csproj" -c Release -o /app/publish

# Step 4: Use the base image and copy the published files
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorApp.dll"]
