# Job Starter



`StarkStarterJob` 是针对`Quartz`的封装。当依赖了 `StartStarterJob`以后，很容易就可以实现一个job



### 1、Job实现

第一步、 创建一个实现IJob接口的类，并在`Execute`里实现自己的业务代码

第二步、 添加TriggerCron特性，配置执行时间

```csharp
[TriggerCron("0 0/1 * * * ?")]
public class TestJob1 : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"{DateTime.Now} TestJob1");
        return Task.CompletedTask;
    }
}
```



### 2、Cron配置

#### 2.1、硬编码写死

```csharp
[TriggerCron("0 0/1 * * * ?")]
```



#### 2.2、配置中读取

```csharp
[TriggerCron("${job:job2Cron}")]
```

配置中读取更灵活，如果读取不到配置，将不执行这个任务