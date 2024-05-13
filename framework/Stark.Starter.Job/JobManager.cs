using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;
using Volo.Abp.DependencyInjection;

namespace Stark.Starter.Job;

public class JobManager : ITransientDependency
{
    private readonly IJobFactory _jobFactory;
    private readonly ISchedulerFactory _schedulerFactory;
    
    public JobManager(
        ISchedulerFactory schedulerFactory,
        IJobFactory jobFactory)
    {
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
    }
    
    public async Task AddJobAsync(Type jobType, string cron, string id = "")
    {
        var name = jobType.FullName + id;

        var scheduler = await _schedulerFactory.GetScheduler();
        scheduler.JobFactory = _jobFactory;

        var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals($"{name}Group"));
        if (jobKeys.Count > 0)
            if (jobKeys.Any(x => x.Name == name))
                return;

        var job = JobBuilder.Create(jobType).WithIdentity(name, $"{name}Group")
            .WithDescription(jobType.Name)
            .UsingJobData("Id", id)
            .Build();

        var trigger = TriggerBuilder
            .Create()
            .WithIdentity($"{name}.trigger", $"{name}Group")
            .WithCronSchedule(cron)
            .WithDescription(jobType.Name)
            .Build();

        await scheduler.ScheduleJob(job, trigger);
        await scheduler.Start();
    }
    
    public async Task GetJobListAsync()
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        
        // 获取所有作业的键
        var jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup()).Result;

        // 打印作业信息
        foreach (var jobKey in jobKeys)
        {
            var jobDetail = scheduler.GetJobDetail(jobKey).Result;
            // 获取作业的触发器
            var triggerKey = new TriggerKey($"{jobDetail.JobType.FullName}.trigger",jobKey.Group);
            var trigger = await scheduler.GetTrigger(triggerKey);
            // 获取下次执行时间
            var nextFireTime = trigger.GetNextFireTimeUtc()?.ToLocalTime();

            // 输出下次执行时间
            Console.WriteLine($"Next Fire Time: {nextFireTime}");
            Console.WriteLine($"Job Name: {jobKey.Name}, Group: {jobKey.Group}, Job Type: {jobDetail.JobType.FullName}");
        }
    }
}