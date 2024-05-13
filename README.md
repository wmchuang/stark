﻿# Stark

Stark是一个极简化的ASP.NET Core应用框架，基于Abp.NET，只引入了`Volo.Abp.Autofac`包来获得Abp的模块化、懒加载注入和属性注入等核心功能。在此基础上，构建了Stark.Web.Core核心模块，实现了JWT认证、Swagger文档生成、过滤器、异常处理和日志记录等Web API框架核心功能。

## 特点

- **极简化**：只引入必要的Abp包，无冗余代码。
- **模块化**：支持模块化开发，便于组织代码和维护。
- **懒加载**：支持依赖注入的懒加载，提升性能。
- **属性注入**：支持通过属性注入依赖，方便快捷。
- **JWT认证**：内置JWT认证功能，便于构建RESTful API。
- **Swagger**：内置Swagger文档生成，便于API的文档化。
- **过滤器**：支持全局和局部过滤器，用于处理常见的业务逻辑。
- **异常处理**：统一处理异常，提供友好的API错误响应。
- **日志记录**：内置日志记录功能，便于问题排查和跟踪。

## 优势

- **轻量级**：相较于完整的Abp框架，Stark更加轻量级，只包含Web API框架的核心功能。
- **快速开发**：提供丰富的开箱即用功能，快速搭建RESTful API项目。
- **易于维护**：遵循单一职责原则，代码结构清晰，易于维护和扩展。
- **安全性**：内置JWT认证功能，提供基本的安全保障。
- **文档化**：内置Swagger文档生成，便于API的文档化和测试。

## 适用场景

适用于需要快速搭建RESTful API的Web应用，特别是那些需要模块化、懒加载、属性注入等功能的项目。同时，Stark也适用于需要简化开发过程和提升开发效率的场景。


## Todo
- [ swagger模块分组]
- [job]
- [mq 延时队列]