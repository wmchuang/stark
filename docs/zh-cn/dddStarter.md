# DDD Starter



 DDD分层架构包括：展现层、应用层、领域层和基础设施层

`StarkStarterDDD`其实就是对这四层中的内容做了一些 提取封装，让我们使用DDD架构开发时，能够更方便



### 1、领域层的封装

领域层中封装了实体和聚合根，聚合根中又自带了领域事件，其中领域事件基于`MediatR`来发布的，在落库之前，会自动发布领域事件

### 2、基础设施层的封装

基础设施层中主要对`EFCore`和`SqlSugar`进行了封装：

对EFCore 封装了抽象的 `StarkDbContext`上下文，里面对数据写入前，会自动设置审计字段，以及软删除的全局过滤。



对`SqlSugar`封装了`IBaseQuery`，能让我们快速使用`sqlSUgar`的方法；同样也实现了软删除全局过滤；



这里推荐针对数据写入的操作都使用`EFCore`； 对数据读取的操作使用`SqlSugar`.