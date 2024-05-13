using DotNetCore.CAP;
using Volo.Abp.DependencyInjection;

namespace Stark.Module.Test.Services;

public class TestCap : ICapSubscribe,ISingletonDependency
{
    [CapSubscribe("test.show.time")]
    public void ReceiveMessage(DateTime time)
    {
        Console.WriteLine("message time is:" + time);
    }
}