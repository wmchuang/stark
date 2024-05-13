using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Stark.Starter.Web.Hub;

/// <summary>
/// 通知集线器
/// </summary>
// [Authorize]
public class NoticeHub  : Hub<INoticeClient>
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("上线");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine("下线");
        return base.OnDisconnectedAsync(exception);
    }
    
   
}