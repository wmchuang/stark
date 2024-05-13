using Stark.Starter.Work.Weixin.Models;
using Volo.Abp.DependencyInjection;

namespace Stark.Starter.Work.Weixin.Client
{
    public interface IAccessTokenClient : ITransientDependency
    {
        /// <summary>
        /// get AccessToken
        /// </summary>
        /// <returns></returns>
        Task<WorkWxTokenCached> GetAccessTokenAsync();
    }
}