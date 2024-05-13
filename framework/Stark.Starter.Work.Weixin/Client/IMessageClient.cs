using Stark.Starter.Work.Weixin.Models.Request;
using Volo.Abp.DependencyInjection;

namespace Stark.Starter.Work.Weixin.Client
{
    public interface IMessageClient : ITransientDependency
    {
        Task<bool> SendMessageAsync(SendMessageRequest vm);
    }
}