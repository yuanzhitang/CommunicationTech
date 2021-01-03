using ChatRoom.Contract;
using System;
using System.ServiceModel;

namespace ChatRoom.Server
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

				Console.WriteLine("Press Any Key to Exit...");
				Console.ReadLine();

				host.Close();
			}
		}

		private static void Host_Opening(object sender, EventArgs e)
		{
			Console.WriteLine("Server Started：{0}", DateTime.Now.ToString());
		}
	}
}
