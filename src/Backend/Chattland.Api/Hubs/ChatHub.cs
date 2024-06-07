using Chattland.Api.DataAccess.Entities;
using Chattland.Api.DataAccess.Repositories.Interfaces;
using Chattland.DataTransferContract.DataTransferTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chattland.Api.Hubs;

//Todo: Här kan man också konfigurera vilka som får skicka meddelanden
[Authorize]
public class ChatHub(IChatMessageRepository messageRepository, IChatConversationRepository conversationRepository, IChatRoomRepository roomRepository) : Hub
{
    public override async  Task OnConnectedAsync()
    {
        var user = Context.User?.Identity != null ? Context.User.Identity.Name: string.Empty;
        if (string.IsNullOrEmpty(user))
        {
            Context.Abort();
            return;
        }
        await roomRepository.AddParticipantToRoom(user, Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public async Task SendMessage(ChatMessageRequest request)
    {
        messageRepository.SetCollectionName(request.Room);
        var newMessage = new ChatMessageDocument
        {
            Sender = request.ChatMessage.Sender,
            Message = request.ChatMessage.Message,
            CreatedAt = DateTime.Now
        };
        await messageRepository.AddOneAsync(newMessage);
        await Clients.All.SendAsync("SendMessage", request);
    }

    public async Task SendDirectMessage(ChatMessageRequest request)
    {
        var sender = Context.User.Identity.Name;

        var receiver = request.ChatMessage.Sender;
        var message = request.ChatMessage.Message;

    }
}