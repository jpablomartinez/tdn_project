# Use official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files into app folder inside the container
COPY ./TodoAPI.csproj ./
RUN dotnet restore

# Copy remaining files and build the project
COPY . ./

# Expose API port
EXPOSE 8080

ENV DOTNET_USE_POLLING_FILE_WATCHER 1
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080", "--project", "/app/TodoAPI.csproj"]
