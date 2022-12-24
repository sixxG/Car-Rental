#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat 

FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
WORKDIR /doc
#EXPOSE 80

#COPY ${source:-obj/Docker/publish} .

FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8-windowsservercore-ltsc2019
WORKDIR /src
COPY CarRental.csproj CarRental.csproj
RUN dotnet restore CarRental.csproj
COPY . .
WORKDIR /src
RUN dotnet build CarRental.csproj -c Release -o /doc

FROM build AS publish
RUN dotnet publish CarRental.csproj -c Release -o /doc

FROM base AS final
WORKDIR /doc
COPY --from=publish /doc .
ENTRYPOINT ["dotnet", "CarRental.dll"]
