using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Chattland.Api.Hubs;

public class ChatHub(IChatMessageRepository repo) : Hub
{
    public async Task SendMessage(ChatMessageRequest message)
    {
        repo.SetCollectionName(message.Room);
        await repo.AddOneAsync(message.ChatMessage);
        await Clients.All.SendAsync("SendMessage", message);
    }
}

public class ChatMessageRequest
{
    public ChatMessage ChatMessage { get; set; }
    public string Room { get; set; }
}