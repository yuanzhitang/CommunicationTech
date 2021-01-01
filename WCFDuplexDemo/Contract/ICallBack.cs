using System.ServiceModel;

namespace Contract
{
	public interface ICallBack
    {
        [OperationContract]
        string UserId();

        [OperationContract(IsOneWay = true)]
        void SayHello(string mes);
    }
}
