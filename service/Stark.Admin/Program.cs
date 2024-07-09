using AgileConfig.Client;
using Stark.Admin;
using Stark.Starter.Core.Extensions;

// 创建一个WebApplication的构建器
var builder = WebApplication.CreateBuilder(args);

// 使用Autofac作为主机容器
builder.Host.UseAutofac();

//使用AgileConfig配置中心
builder.Host.UseAgileConfig(
    //new ConfigClient($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json"),
    new ConfigClient(),
    e => Console.WriteLine($"configs {e.Action}"));
  

// 添加AdminModule应用程序服务
await builder.Services.AddApplicationAsync<AdminModule>();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// 构建WebApplication
var app = builder.Build();

// 初始化应用程序
await app.InitializeApplicationAsync();

// 运行应用程序
await app.RunAsync();