using Microsoft.AspNetCore.SignalR;

namespace AppView.Hubs
{
	public class ConnecttedHub : Hub
	{
		public async Task SendMessage (string user, string message)
		{
			await Clients.All.SendAsync ("ReceiveMessage", user, message);
		}

	}
}
