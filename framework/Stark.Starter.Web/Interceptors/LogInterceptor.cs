using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Stark.Starter.Web.Interceptors;

public class LogInterceptor: AbpInterceptor, ITransientDependency
{
    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        Console.WriteLine("方法操作前日志");
        await invocation.ProceedAsync();
        Console.WriteLine("方法操作后日志");
    }
}