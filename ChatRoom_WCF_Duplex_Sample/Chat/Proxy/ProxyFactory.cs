using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.Chat
{
	class ProxyFactory
	{
		private const string EndpointConfigurationName = "ChatServerEndpoint";

		public static ChatServerProxy CreateChatServerProxy(ChatCallBack chatCallback)
		{
			var context = new InstanceContext(chatCallback);
			var proxy = new ChatServerProxy(context, EndpointConfigurationName);

			return proxy;
		}
	}
}
