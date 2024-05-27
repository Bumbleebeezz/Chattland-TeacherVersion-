using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Chattland.Api.Hubs;

public class ChatHub(IChatMessageRepository repo) : Hub
{
    public async Task SendMessage(ChatMessage message)
    {
        await repo.AddOneAsync(message);
        await Clients.All.SendAsync("SendMessage", message);
    }
}