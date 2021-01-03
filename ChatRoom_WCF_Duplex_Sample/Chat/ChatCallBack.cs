using ChatRoom.Contract;
using System;
using System.Threading;

namespace ChatRoom.Chat
{
	public class ChatCallBack : IChatCallback
	{
		private string userId;

		public event Action<string, string> OnSayHello;
		public event Action<string> OnLogin;
		public event Action<string> OnLogout;

		public ChatCallBack(string userId)
		{
			this.userId = userId;
		}

		public void SayHello(string user, string mes)
		{
			OnSayHello?.Invoke(user, mes);
		}

		public string UserId()
		{
			return userId;
		}

		public void Online(string user)
		{
			OnLogin?.Invoke(user);
		}

		public void Offline(string user)
		{
			OnLogout?.Invoke(user);
		}
	}
}
