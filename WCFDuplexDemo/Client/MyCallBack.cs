using Contract;
using System;
using System.Threading;

namespace Client
{
	public class MyCallBack : ICallBack
	{
		private string userId;

		public MyCallBack(string userId)
		{
			this.userId = userId;
		}

		public void SayHello(string mes)
		{
			Console.WriteLine(mes);
		}

		public string UserId()
		{
			return userId;
		}
	}
}
