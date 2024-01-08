using Microsoft.AspNetCore.SignalR;

namespace TopMai.Domain.Hubs
{
    public class WalletHub : Hub
    {
        public WalletHub()
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
    }
}
