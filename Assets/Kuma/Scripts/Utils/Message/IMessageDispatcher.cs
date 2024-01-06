/*
	IMessage.cs
	Author: Kuma
	Created-Date: 2019-01-06

	-----Description-----
	MessageDispathcer interface.
*/





namespace Kuma.Utils.Message {

	public interface IMessageDispatcher {
		
		void AddListener (MessageType msgType, MessageListenerDelegate listener);

	}	

	public delegate void MessageListenerDelegate (IMessage msg);

}