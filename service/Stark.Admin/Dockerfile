FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["service/Stark.Admin/Stark.Admin.csproj", "service/Stark.Admin/"]
COPY ["framework/Stark.Web.Start/Stark.Web.Start.csproj", "framework/Stark.Web.Start/"]
COPY ["framework/Stark.Starter.DDD/Stark.Starter.DDD.csproj", "framework/Stark.Starter.DDD/"]
COPY ["framework/Stark.Core/Stark.Core.csproj", "framework/Stark.Core/"]
COPY ["service/Modules/AIModule/AIModule.csproj", "service/Modules/AIModule/"]
COPY ["framework/Stark.Redis.Start/Stark.Redis.Start.csproj", "framework/Stark.Redis.Start/"]
COPY ["framework/Stark.Work.Weixin.Start/Stark.Work.Weixin.Start.csproj", "framework/Stark.Work.Weixin.Start/"]
COPY ["framework/Stark/Stark.csproj", "framework/Stark/"]
COPY ["service/Modules/InfModule/InfModule.csproj", "service/Modules/InfModule/"]
COPY ["framework/Stark.Cap.Start/Stark.Cap.Start.csproj", "framework/Stark.Cap.Start/"]
COPY ["service/Modules/SystemModule/SystemModule.csproj", "service/Modules/SystemModule/"]
COPY ["service/Modules/TestModule/TestModule.csproj", "service/Modules/TestModule/"]
COPY ["framework/Stark.Job.Start/Stark.Job.Start.csproj", "framework/Stark.Job.Start/"]
RUN dotnet restore "service/Stark.Admin/Stark.Admin.csproj"
COPY . .
WORKDIR "/src/service/Stark.Admin"
RUN dotnet build "Stark.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Stark.Admin.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN rm -f /etc/localtime \
&& ln -sv /usr/share/zoneinfo/Asia/Shanghai /etc/localtime \
&& echo "Asia/Shanghai" > /etc/timezone
ENTRYPOINT ["dotnet", "Stark.Admin.dll"]
