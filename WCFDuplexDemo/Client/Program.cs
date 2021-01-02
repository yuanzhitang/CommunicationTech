using Contract;
using System;
using System.ServiceModel;

namespace Client
{
	class Program
	{
		//private const string ClientAddress = "http://localhost:";
		private const string ServerAddress = "net.tcp://localhost:9099";

		static void Main(string[] args)
		{
			//Console.Write($"Enter a Port:");
			//var port = Console.ReadLine();
			Console.Write($"Enter your name:");
			var user = Console.ReadLine();

			ICallBack callback = new MyCallBack(user);
			InstanceContext context = new InstanceContext(callback);
			NetTcpBinding binding = new NetTcpBinding();
			//binding.ClientBaseAddress = new Uri(ClientAddress+port);
			using (var proxy = new DuplexChannelFactory<IMessageService>(context, binding))
			{
				IMessageService client = proxy.CreateChannel(new EndpointAddress(ServerAddress));
				client.RegisterClient();

				while (true)
				{
					Console.Write($"[{user}]:");
					var msg = Console.ReadLine();
					client.SendMessage(msg, user);
				}
			}
		}
	}
}
