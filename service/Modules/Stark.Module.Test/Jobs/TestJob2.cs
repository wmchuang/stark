using Quartz;
using Stark.Starter.Job.Attribute;

namespace Stark.Module.Test.Jobs;

[TriggerCron("${job:job2Cron}")]
public class TestJob2 : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"{DateTime.Now} TestJob2");
        return Task.CompletedTask;
    }
}