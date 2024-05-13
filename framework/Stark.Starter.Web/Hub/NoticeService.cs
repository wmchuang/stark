using Microsoft.AspNetCore.SignalR;

namespace Stark.Starter.Web.Hub;

public class NoticeService : INoticeService
{
    private readonly IHubContext<NoticeHub, INoticeClient> _hubContext;

    public NoticeService(IHubContext<NoticeHub, INoticeClient> hubContext)
    {
        _hubContext = hubContext;
    }

    ///<inheritdoc cref="SendToAllClientsAsync"/>
    public async Task SendToAllClientsAsync(object vm)
    {
        await _hubContext.Clients.All.SendToAllClients(vm);
    }

    ///<inheritdoc cref="SendToExceptClientsAsync"/>
    public async Task SendToExceptClientsAsync(string exceptConnId, object vm)
    {
        await _hubContext.Clients.AllExcept(exceptConnId).SendToExceptClients(vm);
    }

    ///<inheritdoc cref="SendToClientAsync(string, object)"/>
    public async Task SendToClientAsync(string connId, object vm)
    {
        await _hubContext.Clients.Client(connId).SendToClient(vm);
    }
    
    ///<inheritdoc cref="SendToUserAsync(string, object)"/>
    public async Task SendToUserAsync(string userId, object vm)
    {
        await _hubContext.Clients.User(userId).SendToUser(vm);
    }

    ///<inheritdoc cref="SendToUsersAsync(List{string}, object)"/>
    public async Task SendToUsersAsync(List<string> userIds, object vm)
    {
        await _hubContext.Clients.Users(userIds).SendToUsers(vm);
    }
}