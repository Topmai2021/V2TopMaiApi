using Microsoft.AspNetCore.SignalR;

namespace TopMai.Domain.Hubs
{
    public class MessageHub : Hub
    {
        public MessageHub()
        {

        }
        public override Task OnConnectedAsync()
        {
            _ = this.Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        /**
        public Task Connect(Profile profile,string connectionId)
        {

            if(profile != null)
            {
                profile.ConnectionId = connectionId;
                ProfileService.Put(profile);

            }

            return Clients.Client(connectionId).SendAsync("connected",connectionId);
        }

        public Task NotifyMessage( Message message)
        {

            // connection id of sender
            string idFrom = message.From.ConnectionId;

            // get connection id of receiver
            string idTo;
            if (idFrom == message.Chat.ProfileOne.ConnectionId)
            {
                idTo = message.Chat.ProfileOne.ConnectionId;
            }
            else
            {
                idTo = message.Chat.ProfileTwo.ConnectionId;

            }

            return (Task)Clients.Client(idTo).SendAsync("newMessage", new {value=message.Id});

        }

                **/

    }

}
