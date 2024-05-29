using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Chattland.Api;

public class ChatHub(IChatMessageRepository repo) : Hub
{
    public async Task SendMessage(ChatMessageDocument message)
    {
        await repo.AddOneAsync(message);
        await Clients.All.SendAsync("SendMessage", message);
    }
}