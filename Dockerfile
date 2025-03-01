FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
COPY . .

RUN dotnet test
RUN dotnet publish Knab.Api/Knab.Api.csproj -c Release -o /app/out


FROM mcr.microsoft.com/dotnet/aspnet
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Knab.Api.dll"]

