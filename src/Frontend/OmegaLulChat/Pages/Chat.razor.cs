using System.Net.Http.Json;
using Chattland.DataTransferContract.DataTransferTypes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using OmegaLulChat.Auth;

namespace OmegaLulChat.Pages;

//Todo: Bröt ut code behind för ökad läsbarhet
public partial class Chat
{
    private string _room;

    [Parameter]
    public string Room
    {
        get => _room;
        set
        {
            _room = value;
            Task.Run(GetMessages);
        }
    }

    private async Task GetMessages()
    {
        var client = _httpClientFactory.CreateClient("chatApi");
        var response = await client.GetAsync($"/messages/{Room}?start=0&count=0");

        if (response.IsSuccessStatusCode)
        {
            var messages = await response.Content.ReadFromJsonAsync<ChatMessage[]>();
            _messages.Clear();
            _messages.AddRange(messages ?? []);
            StateHasChanged();
        }
    }

    private List<ChatMessage> _messages = new();
    private ChatMessage _newChatMessage = new() {Sender = new MessageSender()};
    private HubConnection _hubConnection;

    protected override async Task OnInitializedAsync()
    {
        await GetMessages();

        _hubConnection =
            new HubConnectionBuilder()
                .WithUrl("https://localhost:7194/hubs/Chat", options =>
                {
                    //Todo: Lägger till vår klass som skickar med credentials
                    options.HttpMessageHandlerFactory = innerHandler =>
                        new IncludeRequestCredentialsMessageHandler {InnerHandler = innerHandler};
                })
                .Build();

        _hubConnection.On<ChatMessageRequest>("SendMessage", (message) =>
        {
            if (message.Room.Equals(Room))
            {
                _messages.Add(message.ChatMessage);
                StateHasChanged();
            }
        });

        //Todo: Lägger till användarnamn
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _newChatMessage.Sender.Name = authState.User.Identity.Name;

        await _hubConnection.StartAsync();

        await base.OnInitializedAsync();
    }

    private async Task Callback()
    {
        _newChatMessage.CreatedAt = DateTime.UtcNow;
        var message = new ChatMessageRequest() {ChatMessage = _newChatMessage, Room = Room};
        await _hubConnection.SendAsync($"SendMessage", message);
    }
}