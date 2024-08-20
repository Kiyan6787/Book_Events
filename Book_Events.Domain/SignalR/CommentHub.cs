using Book_Events.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Book_Events.Domain.SignalR
{
    public class CommentHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }

        public async Task SendCommentNotification(string user, string message)
        {
            await Clients.Group(user).SendAsync("ReceiveCommentNotification", Context.User.Identity.Name, message);
        }
    }
}
