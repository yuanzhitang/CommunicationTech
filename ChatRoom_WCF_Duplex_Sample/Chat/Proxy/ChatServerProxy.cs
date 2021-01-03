using ChatRoom.Contract;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ChatRoom.Chat
{
	public class ChatServerProxy : ClientBase<IMessageService>, IMessageService
	{
		public ChatServerProxy()
		{
		}

		public ChatServerProxy(Binding binding, EndpointAddress remoteAddress)
			: base(binding, remoteAddress)
		{
		}

		public ChatServerProxy(string endpointConfigurationName, EndpointAddress remoteAddress)
			: base(endpointConfigurationName, remoteAddress)
		{
		}

		public ChatServerProxy(InstanceContext callbackInstance, string endpointConfigurationName) : base(callbackInstance,endpointConfigurationName)
		{
		}

		public void RegisterClient()
		{
			Channel.RegisterClient();
		}

		public void SendMessage(string msg, string userId)
		{
			Channel.SendMessage(msg, userId);
		}
	}
}
