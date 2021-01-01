using System.ServiceModel;

namespace Contract
{
	[ServiceContract(CallbackContract = typeof(ICallBack))]
	public interface IMessageService
	{
		[OperationContract]
		void RegisterClient();

		[OperationContract]
		void SendMessage(string msg, string userId);
	}
}
