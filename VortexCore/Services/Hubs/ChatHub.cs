using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using VortexCore.ModelsDB.MongoDB;
using VortexCore.Services.MongoDB;
using MongoDB.Bson;
using System.Security.Claims;
using VortexCore.DtoClasses;
using System.Collections.ObjectModel;

namespace VortexCore.Services.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        private readonly static Dictionary<string ,User> clients = new Dictionary<string, User>();

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }


        public override async Task OnConnectedAsync() 
        {
            var allMessges = _chatService.GetMessages();
            var user = await User.GetUser(Context.User);
            clients.Add(Context.ConnectionId, user);
            await Clients.Caller.SendAsync("ChatReady", new {
                messages = allMessges,
                clients = clients.Values
            });
            await Clients.Others.SendAsync("UserJoin", user);
            
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception) 
        {
            var user = clients[Context.ConnectionId];
            clients.Remove(Context.ConnectionId);
            await Clients.Others.SendAsync("UserLeave", user);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string message)
        {
            var uid = Context.User.FindFirstValue("user_id");
            await Clients.All.SendAsync(
                "ReceiveMessage", 
                _chatService.AddMessage(new ChatMessage
                    {
                        MessageText = message,
                        UserId = uid
                    })
                );
        }
    }

}
