using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace Stark.Starter.Job;

public class QuartzHostedService : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IEnumerable<JobSchedule> _jobSchedules;
    private readonly IJobFactory _jobFactory;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="schedulerFactory"></param>
    /// <param name="jobSchedules"></param>
    /// <param name="jobFactory"></param>
    public QuartzHostedService(
        ISchedulerFactory schedulerFactory,
        IEnumerable<JobSchedule> jobSchedules,
        IJobFactory jobFactory)
    {
        _schedulerFactory = schedulerFactory;
        _jobSchedules = jobSchedules;
        _jobFactory = jobFactory;
    }

    private IScheduler Scheduler { get; set; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("QuartzHostedService Run");
        Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        Scheduler.JobFactory = _jobFactory;

        //添加监听
        // Scheduler.ListenerManager.AddJobListener(new CustomJobListener());
        // Scheduler.ListenerManager.AddTriggerListener(new CustomTriggerListener());
        // Scheduler.ListenerManager.AddSchedulerListener(new CustomSchedulerListener());

        foreach (var jobSchedule in _jobSchedules)
        {
            if (jobSchedule.StartNow)
            {
                var nowJob = CreateJob(jobSchedule, "_Now");
                var nowTrigger = CreateNowTrigger();
                await Scheduler.ScheduleJob(nowJob, nowTrigger, cancellationToken);
            }

            var job = CreateJob(jobSchedule);
            var trigger = CreateTrigger(jobSchedule);

            await Scheduler.ScheduleJob(job, trigger, cancellationToken);
        }

        await Scheduler.Start(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Scheduler?.Shutdown(cancellationToken);
    }

    private static IJobDetail CreateJob(JobSchedule schedule, string now = "")
    {
        var jobType = schedule.JobType;
        return JobBuilder
            .Create(jobType)
            .WithIdentity($"{jobType.FullName}{now}", schedule.Group)
            .WithDescription(schedule.JobDesc)
            .Build();
    }

    private static ITrigger CreateTrigger(JobSchedule schedule)
    {
        return TriggerBuilder
            .Create()
            .WithIdentity($"{schedule.JobType.FullName}.trigger", schedule.Group)
            .WithCronSchedule(schedule.CronExpression)
            .WithDescription(schedule.CronExpression)
            .Build();
    }

    private static ITrigger CreateNowTrigger()
    {
        return TriggerBuilder
            .Create()
            .StartNow()
            .Build();
    }
}