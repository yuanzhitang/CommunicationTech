using Contract;
using System;
using System.ServiceModel;

namespace Server
{
	class Program
	{
		private const string Address = "net.tcp://localhost:9099";

		static void Main(string[] args)
		{
			using (ServiceHost host = new ServiceHost(typeof(MessageService)))
			{
				host.AddServiceEndpoint(typeof(IMessageService), new NetTcpBinding(), Address);
				host.Opening += Host_Opening; 
				host.Open();

				while (true)
				{
					Console.WriteLine("Commands:");
					Console.WriteLine("[notify]: Notify all clients");
					Console.WriteLine("[close] : Close the Server");
					Console.Write("Enter command:");
					string command = Console.ReadLine();
					switch (command)
					{
						case "notify":
							lock (MessageService.CallBackList)
							{
								MessageService.NotifyClients();
							}
							break;
						case "close":
							host.Close();
							break;
						default:
							break;
					}
				}
			}
		}

		private static void Host_Opening(object sender, EventArgs e)
		{
			Console.WriteLine("Server Started：{0}", DateTime.Now.ToString());
		}
	}
}
