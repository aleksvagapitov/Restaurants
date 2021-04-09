FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build-image
 
WORKDIR /app
 
COPY ./*.sln  ./
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

RUN dotnet restore
 
COPY . .

# RUN dotnet test ./Tests/Tests.csproj
RUN dotnet publish ./src/API/API.csproj -o /publish/ --no-cache
 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /publish
COPY --from=build-image /publish .
ENV ASPNETCORE_URLS=http://*:$PORT

ENTRYPOINT ["dotnet", "API.dll"]
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet API.dll
