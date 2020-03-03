#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80

#Modify the environment variables accordingly.
ENV PUBLISHER_NAME=Plorence
ENV PUBLISHER_EMAIL=ploseok@gmail.com
ENV GITUSER_URL=https://github.com/zxc010613

RUN apt-get update \ 
	&& apt-get install -y mingw-w64

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["mcode_generator.csproj", ""]
RUN dotnet restore "./mcode_generator.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "mcode_generator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "mcode_generator.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mcode_generator.dll"]