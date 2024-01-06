/*
	IMessage.cs
	Author: Kuma
	Created-Date: 2019-01-06
	
	-----Description-----
	Message interface.
*/





using System;

namespace Kuma.Utils.Message {

	public interface IMessage {
		
		/// <summary>
		/// 消息類型, Key
		/// </summary>
		MessageType MsgType { get; set; }

		/// <summary>
		/// 發送者
		/// </summary>
		Object Sender { get; set; }

		/// <summary>
		/// 參數
		/// </summary>
		Object[] Params { get; set; }
		
	}

}