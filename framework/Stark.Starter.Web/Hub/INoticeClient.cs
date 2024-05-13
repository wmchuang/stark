namespace Stark.Starter.Web.Hub;

/// <summary>
/// 
/// </summary>
public interface INoticeClient
{
    /// <summary>
    /// 给所有人推送消息
    /// </summary>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToAllClients(object vm);

    /// <summary>
    /// 除指定连接以外所有连接推送
    /// </summary>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToExceptClients(object vm);

    /// <summary>
    /// 给指定连接推送事件
    /// </summary>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToClient(object vm);
    
    /// <summary>
    /// 给指定userId发消息
    /// </summary>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToUser(object vm);

    /// <summary>
    /// 给多个指定userId发消息
    /// </summary>
    /// <param name="vm">消息内容</param>
    /// <returns></returns>
    Task SendToUsers(object vm);
}