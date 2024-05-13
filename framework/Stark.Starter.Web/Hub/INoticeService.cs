using Volo.Abp.DependencyInjection;

namespace Stark.Starter.Web.Hub;

public interface INoticeService : ITransientDependency
{
    /// <summary>
    /// 给所有人推送消息
    /// </summary>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToAllClientsAsync(object vm);

    /// <summary>
    /// 除指定连接以外所有连接推送
    /// </summary>
    /// <param name="exceptConnId">除了指定连接</param>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToExceptClientsAsync(string exceptConnId, object vm);

    /// <summary>
    /// 给指定连接推送事件
    /// </summary>
    /// <param name="connId">用户连接</param>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToClientAsync(string connId, object vm);
    
    
    /// <summary>
    /// 给指定userId发消息
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToUserAsync(string userId, object vm);

    /// <summary>
    /// 给多个指定userId发消息
    /// </summary>
    /// <param name="userIds">用户ID集合</param>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToUsersAsync(List<string> userIds, object vm);
}