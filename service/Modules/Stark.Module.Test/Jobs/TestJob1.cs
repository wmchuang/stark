using Quartz;
using Stark.Starter.Job.Attribute;

namespace Stark.Module.Test.Jobs;

[TriggerCron("0 0/1 * * * ?")]
public class TestJob1 : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"{DateTime.Now} TestJob1");
        return Task.CompletedTask;
    }
}