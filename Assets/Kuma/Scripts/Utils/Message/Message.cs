/*
	IMessage.cs
	Author: Kuma
	Created-Date: 2019-01-06

	-----Description-----
	IMessage implemention.
*/





using System;

namespace Kuma.Utils.Message {

	public class Message : IMessage {
		
		public MessageType MsgType { get; set; }
		public Object Sender { get; set; }
		public Object[] Params { get; set; }

		public override string ToString() {
			string str = null;

			if (Params != null) {
				int length = Params.Length;
				for (int i = 0; i < length; i++) {
					str += Params[i];
					if (i != (length - 1)) {
						str += ", ";
					}

				}
			}

			return String.Format ("MessageType:{0}\nSender:{1}\nParams:[{2}]\n\n\n", MsgType, Sender == null ? "Null" : Sender, str);
		}
	
		public Message Clone() {
			return new Message(MsgType, Params, Sender);
		}
	
		public Message(MessageType type) {
			MsgType = type;
			Params = null;
			Sender = null;
		}
	
		public Message(MessageType type, Object sender, params Object[] param) {
			MsgType = type;
			Params = param;
			Sender = sender;
		}

		public void Send () {
			MessageCenter.Default.SendMessage (this);
		}

	}

}