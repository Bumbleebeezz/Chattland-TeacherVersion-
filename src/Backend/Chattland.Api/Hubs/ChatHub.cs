using Chattland.Api.DataAccess;
using Chattland.Api.DataAccess.Entities;
using Chattland.DataTransferContract.DataTransferTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chattland.Api.Hubs;

[Authorize]
public class ChatHub(IChatMessageRepository repo) : Hub
{
    public async Task SendMessage(ChatMessageRequest request)
    {
        repo.SetCollectionName(request.Room);
        var newMessage = new ChatMessageDocument
        {
            Sender = request.ChatMessage.Sender,
            Message = request.ChatMessage.Message,
            CreatedAt = DateTime.Now
        };
        await repo.AddOneAsync(newMessage);
        await Clients.All.SendAsync("SendMessage", request);
    }
}