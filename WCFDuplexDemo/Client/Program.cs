using Contract;
using System;
using System.ServiceModel;

namespace Client
{
	class Program
	{
		private const string ClientAddress = "http://localhost:";
		private const string ServerAddress = "http://localhost:9099";

		static void Main(string[] args)
		{
			Console.Write($"Enter a Port:");
			var port = Console.ReadLine();
			Console.Write($"Enter a User:");
			var user = Console.ReadLine();

			ICallBack callback = new MyCallBack(user);
			InstanceContext context = new InstanceContext(callback);
			WSDualHttpBinding binding = new WSDualHttpBinding();
			binding.ClientBaseAddress = new Uri(ClientAddress+port);
			using (var proxy = new DuplexChannelFactory<IMessageService>(context, binding))
			{
				IMessageService client = proxy.CreateChannel(new EndpointAddress(ServerAddress));
				client.RegisterClient();

				while (true)
				{
					Console.Write($"[{user}], Enter a Message:");
					var msg = Console.ReadLine();
					client.SendMessage(msg, user);
				}
			}
		}
	}
}
