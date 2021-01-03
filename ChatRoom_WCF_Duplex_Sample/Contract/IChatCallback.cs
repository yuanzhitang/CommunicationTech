using System.ServiceModel;

namespace ChatRoom.Contract
{
	public interface IChatCallback
    {
        [OperationContract]
        string UserId();

        [OperationContract(IsOneWay = true)]
        void SayHello(string user, string mes);

        [OperationContract(IsOneWay = true)]
        void Online(string user);

        [OperationContract(IsOneWay = true)]
        void Offline(string user);
    }
}
