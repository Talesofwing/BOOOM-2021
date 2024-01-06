/*
	AssetLoader.cs
	Author: Kuma
	Created-Date: 2019-03-08

	-----Description-----
	Load assets with UnityWebRequest.
*/ 





using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

using Kuma.Common;

namespace Kuma.Utils {

	public class AssetLoader {
		
		private static IEnumerator _load (string url, Action<float> processing, Action<UnityWebRequest> finished, Action<string> failure) {
			UnityWebRequest webRequest = new UnityWebRequest (url);
			webRequest.downloadHandler = new DownloadHandlerTexture ();
			var async = webRequest.SendWebRequest ();
			if (webRequest.isHttpError || webRequest.isNetworkError) {
				if (failure != null) {
					failure (webRequest.error);
				}
				Debug.LogWarning (webRequest.error);
				yield break;
			}
			
			while (!webRequest.isDone) {
				if (processing != null) {
					processing (async.progress);
				}
				yield return null;
			}

			if (finished != null) {
				finished (webRequest);
			}
		}

		public static string GetAbsoluteLocalURL(string path) {
			string url = "";

			// Set path.
		#if UNITY_STANDALONE_WIN
			url = "file://" + path;
		#elif UNITY_ANDROID
			url = "file:///" + path;
		#elif UNITY_IOS
			url = "file:///" + path;
		#else
			url = "file://" + path;
			Debug.LogWarning ("Unkonwn platform.");
		#endif

			return url;
		}

		public static void Load (string url, Action<UnityWebRequest> finished) {
			Load (url, null, finished, null);
		}

		public static void Load (string url, Action<float> processing, Action<UnityWebRequest> finished) {
			Load (url, processing, finished, null);
		}

		public static void Load (string url, Action<float> processing, Action<UnityWebRequest> finished, Action<string> failure) {
			CoroutineController.Instance.StartCoroutine (_load (url, processing, finished, failure));
		}

	}

}
