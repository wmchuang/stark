FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["service/Stark.Admin/Stark.Admin.csproj", "service/Stark.Admin/"]
COPY ["framework/Stark.Starter.Web/Stark.Starter.Web.csproj", "framework/Stark.Starter.Web/"]
COPY ["framework/Stark.Starter.Core/Stark.Starter.Core.csproj", "framework/Stark.Starter.Core/"]
COPY ["service/Modules/Stark.Module.AI/Stark.Module.AI.csproj", "service/Modules/Stark.Module.AI/"]
COPY ["framework/Stark.Starter.DDD/Stark.Starter.DDD.csproj", "framework/Stark.Starter.DDD/"]
COPY ["framework/Stark.Starter.Redis/Stark.Starter.Redis.csproj", "framework/Stark.Starter.Redis/"]
COPY ["framework/Stark.Starter.Work.Weixin/Stark.Starter.Work.Weixin.csproj", "framework/Stark.Starter.Work.Weixin/"]
COPY ["framework/Stark/Stark.csproj", "framework/Stark/"]
COPY ["service/Modules/Stark.Module.Inf/Stark.Module.Inf.csproj", "service/Modules/Stark.Module.Inf/"]
COPY ["framework/Stark.Starter.Cap/Stark.Starter.Cap.csproj", "framework/Stark.Starter.Cap/"]
COPY ["service/Modules/Stark.Module.System/Stark.Module.System.csproj", "service/Modules/Stark.Module.System/"]
COPY ["service/Modules/Stark.Module.Test/Stark.Module.Test.csproj", "service/Modules/Stark.Module.Test/"]
COPY ["framework/Stark.Starter.Job/Stark.Starter.Job.csproj", "framework/Stark.Starter.Job/"]
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
