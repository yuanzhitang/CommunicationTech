using Contract;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Server
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public class MessageService : IMessageService, IDisposable
	{
		public static List<ICallBack> CallBackList
		{
			get;
			set;
		}

		public MessageService()
		{
			CallBackList = new List<ICallBack>();
		}

		public void RegisterClient()
		{
			ICallBack callback = OperationContext.Current.GetCallbackChannel<ICallBack>();
			string sessionid = OperationContext.Current.SessionId;
			var ms = OperationContext.Current.IncomingMessageProperties;
			var remp = ms[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;

			Console.WriteLine($"SessionId: {sessionid}, User:{callback.UserId()}, Address:{remp.Address}, Port:{remp.Port} is registered");
			OperationContext.Current.Channel.Closing +=
				delegate
				{
					lock (CallBackList)
					{
						CallBackList.Remove(callback);
						Console.WriteLine("SessionId: {0}, User:{1} is removed", sessionid, callback.UserId());
					}
				};

			CallBackList.Add(callback);
		}

		public void SendMessage(string msg, string userId)
		{
			Console.WriteLine($"{userId}:{msg}");

			NotifyClients(userId);
		}

		public static void NotifyClients(string userId="")
		{
			lock (MessageService.CallBackList)
			{
				foreach (ICallBack callback in MessageService.CallBackList)
				{
					if (!string.IsNullOrEmpty(userId) && callback.UserId() == userId)
					{
						callback.SayHello($"Welcome: {userId}");
					}
				}
			}
		}

		public void Dispose()
		{
			CallBackList.Clear();
		}
	}
}
