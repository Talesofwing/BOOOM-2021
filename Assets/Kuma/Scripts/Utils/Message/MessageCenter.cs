/*
	IMessage.cs
	Author: Kuma
	Created-Date: 2019-01-06

	-----Description-----
	IMessageDispatcher implemention.
*/





using System;
using System.Collections.Generic;

using UnityEngine;

namespace Kuma.Utils.Message {

	public class MessageCenter : IMessageDispatcher {

		private static MessageCenter _defaultCenter = null;
		public static MessageCenter Default { 
			get {
				if (_defaultCenter == null) {
					_defaultCenter = new MessageCenter ();
				}

				return  _defaultCenter;
			}
		}	

		private Dictionary<MessageType, MessageListenerDelegate> _listeners = new Dictionary<MessageType, MessageListenerDelegate> ();

		public void AddListener (MessageType msgType, MessageListenerDelegate listener) {
			if (listener == null) {
				Debug.LogWarning ("AddListener listener is null.");
				return;
			}

			MessageListenerDelegate l;
			_listeners.TryGetValue (msgType, out l);
			_listeners[msgType] = (MessageListenerDelegate)Delegate.Combine (l, listener);
		}

		public void RemoveListener (MessageType msgType, MessageListenerDelegate listener) {
			if (listener == null) {
				Debug.LogWarning ("RemoveListener listener is null.");
				return;
			}

			MessageListenerDelegate l;
			_listeners.TryGetValue (msgType, out l);
			_listeners[msgType] = (MessageListenerDelegate)Delegate.Remove (l, listener);
			if (_listeners[msgType] == null) {
				_listeners.Remove (msgType);
			}
		}

		public void Clear () {
			_listeners.Clear ();
		}

		public void SendMessage (Message msg) {

			if (_listeners.ContainsKey (msg.MsgType)) {
				_listeners [msg.MsgType] (msg);
			}

		}

		public void SendMessage (MessageType msgType, System.Object sender, params System.Object[] param) {
			Message msg = new Message (msgType, sender, param);
			SendMessage (msg);
		}

	}

}