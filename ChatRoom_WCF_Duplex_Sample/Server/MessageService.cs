using ChatRoom.Contract;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ChatRoom.Server
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public class MessageService : IMessageService, IDisposable
	{
		public static List<IChatCallback> CallBackList
		{
			get;
			set;
		}

		public MessageService()
		{
			CallBackList = new List<IChatCallback>();
		}

		public void RegisterClient()
		{
			string sessionid = OperationContext.Current.SessionId;
			var ms = OperationContext.Current.IncomingMessageProperties;
			var remp = ms[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
			IChatCallback currentLoginUserCallBack = OperationContext.Current.GetCallbackChannel<IChatCallback>();
			var loginUser = currentLoginUserCallBack.UserId();

			Console.WriteLine($"SessionId: {sessionid}, User:{loginUser}, Address:{remp.Address}, Port:{remp.Port} is registered");

			RegisterChannelClosingEvent(sessionid,loginUser, currentLoginUserCallBack);
			CallBackList.Add(currentLoginUserCallBack);
			lock (CallBackList)
			{
				foreach (var callback in CallBackList)
				{
					callback.Online(loginUser);

					var otherUser = callback.UserId();
					if(!loginUser.Equals(otherUser))
					{
						currentLoginUserCallBack.Online(otherUser);
					}
				}
			}
		}

		private static void RegisterChannelClosingEvent(string sessionid,string userId, IChatCallback callback)
		{
			OperationContext.Current.Channel.Closing +=
				delegate
				{
					lock (CallBackList)
					{
						Console.WriteLine($"SessionId: {sessionid}, User:{userId} is removed");
						foreach (IChatCallback callbackItem in CallBackList)
						{
							if (callbackItem != callback)
							{
								callbackItem.Offline(userId);
							}
						}

						CallBackList.Remove(callback);
					}
				};
		}

		public void SendMessage(string msg, string userId)
		{
			Console.WriteLine($"{userId}:{msg}");
			NotifyClients(userId, msg);
		}

		public static void NotifyClients(string userId, string msg)
		{
			lock (CallBackList)
			{
				foreach (IChatCallback callback in CallBackList)
				{
					callback.SayHello(userId, msg);
				}
			}
		}

		public void Dispose()
		{
			CallBackList.Clear();
		}
	}
}
