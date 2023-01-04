# Pull asp.net sdk base image
FROM mcr.microsoft.com/dotnet/sdk

# Install .net dependencies
WORKDIR /aspnet
COPY ./asp.net.csproj .
RUN dotnet restore
RUN dotnet add package Microsoft.AspNetCore.OpenApi --version 7.0.1
RUN dotnet add package Swashbuckle.AspNetCore --version 6.4.0

# Move to container
COPY . .

# Launch app
CMD ["dotnet", "watch"]
EXPOSE 5074