using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatRoomSite.Hubs
{
	public class ChatHub : Hub
	{
		//private IList<string> Users = new List<string>();
		public async Task Login(string name)
		{
			await Clients.AllExcept(Context.ConnectionId).
				SendAsync("Login", name);
			//foreach(var user in Users)
			//{
			//	await Clients.Client(Context.ConnectionId).SendAsync("Login", user);
			//}

			//if (!Users.Contains(name))
			//{
			//	Users.Add(name);
			//}
		}

		public async Task SignOut(string name)
		{
			await Clients.AllExcept(Context.ConnectionId)
				.SendAsync("SignOut", $"{name} 离开了群聊！");
		}

		public async Task SendMessage(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, message);
		}

		public async Task SendMessageByServer(string user, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", user, "系统通知:" + message);
		}
	}
}
