using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Stark.Starter.Job.Attribute;
using Volo.Abp.Modularity;

namespace Stark.Starter.Job;

public class StarkStarterJob : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddSingleton<IJobFactory, QuartzJobFactory>();
        context.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

        #region Add JobSchedule

        var jobTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
                a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob)) && t.IsClass && !t.IsAbstract))
            .ToArray();

        // 获取配置
        var configuration = context.Services.GetConfiguration();

        foreach (var jobType in jobTypes)
        {
            context.Services.AddTransient(jobType);

            var cron = GetCron(jobType, configuration);
            if (string.IsNullOrWhiteSpace(cron)) continue;

            var name = jobType.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>()?.Description ?? jobType.Name;
            var schedule = new JobSchedule(jobType, cron, name, $"{jobType.Name}Group", false);
            context.Services.AddSingleton(schedule);
        }

        #endregion

        context.Services.AddHostedService<QuartzHostedService>();
    }

    private string? GetCron(Type jobType, IConfiguration configuration)
    {
        var triggerCron = jobType.GetCustomAttributes().OfType<TriggerCronAttribute>().FirstOrDefault();
        if (triggerCron == null)
            return null;

        var cron = triggerCron.Cron;

        //如果cron以${开头 }结尾，说明是从配置文件获取
        if (cron.StartsWith("${") && cron.EndsWith("}"))
        {
            //${job:job2:cron}  获取{}中间的字符
            cron = cron.Substring(2, cron.Length - 3);
            cron = configuration.GetValue<string>(cron);
        }

        return cron;
    }
}