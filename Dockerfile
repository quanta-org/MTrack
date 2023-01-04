# Pull node base image
FROM node:19-alpine AS nodebuild

# Install node dependencies
WORKDIR /nodebuild
COPY /svelte/package.json .
RUN npm install

# Build node app
COPY /svelte/. .
RUN npm run build

# Pull asp.net sdk base image
FROM mcr.microsoft.com/dotnet/sdk AS netbuild

# Install .net dependencies
WORKDIR /netbuild
COPY /asp.net/asp.net.csproj .
RUN dotnet restore
RUN dotnet add package Microsoft.AspNetCore.OpenApi --version 7.0.1

# Build .net app
COPY /asp.net/. .
RUN dotnet publish -c release -o /release

# Deploy .net server
FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
COPY --from=netbuild /release/. .
COPY --from=nodebuild /nodebuild/build/. wwwroot
CMD ["dotnet", "asp.net.dll"]
EXPOSE 5000