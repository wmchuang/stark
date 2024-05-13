# 快速开始

通过本章的讲述，您将了解：

1. 如何快速使用Stark创建一个项目
2. 如何快速通过Code First方式实现一个REST API



## 工程环境

- Net7
- MySql



## 使用流程



### 1.  初始化项目

新建WebApi项目，添加nuget包

```xml
  dotnet add package Stark.Module.System
```



appsettings.json中添加数据库配置

```json
  "ConnectionStrings": {
    "DbType": "MySql",
    "ConnectionConfigs": "Server=localhost;Port=3306;Database=starkTest;uid=root;pwd=123456;Charset=utf8mb4;"
  }
```



新建一个`StartModule`  继承于`AbpModule`,同时配置依赖于 `StarkSystemModule`

```c#
[DependsOn(typeof(StarkSystemModule))]
public class StartModule : AbpModule
{
}
```



修改Program文件，使其从StartModule开始启动

```csharp
var builder = WebApplication.CreateBuilder(args);

// 使用Autofac作为主机容器
builder.Host.UseAutofac();

// 添加StartModule应用程序服务
await builder.Services.AddApplicationAsync<StartModule>();

// 构建WebApplication
var app = builder.Build();

// 初始化应用程序
await app.InitializeApplicationAsync();

// 运行应用程序
await app.RunAsync();
```



启动项目，将可以看到 已集成了一个通用的RBAC系统设置模块。

![image-20240428134914366](..\img\image-20240428134914366.png)





如果想在swagger上看到详细的描述，需要在 项目配置中添加一下配置项 `CopyDocumentationFilesFromPackages`
将此属性设置为 true 时，将项目中 PackageReference 项的所有生成的 XML 文档文件复制到生成输出。 请注意，启用此功能将导致部署捆绑包大小增加。此属性是在 .NET SDK 7.0.100 中引入的

```xml
<PropertyGroup>
	<CopyDocumentationFilesFromPackages>true</CopyDocumentationFilesFromPackages>
</PropertyGroup> 
```