using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatRoomLogin : MonoBehaviour {

	static string appKey = "734fed5d1ff568d31042f0f05407d428";
	bool _loginSuccess;
	
	private static string _appDataPath;
	public static string AppDataPath
	{
		get
		{
			if (string.IsNullOrEmpty(_appDataPath))
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					_appDataPath = Application.persistentDataPath + "/NimUnityDemo";
				}
				else if (Application.platform == RuntimePlatform.Android)
				{
					string androidPathName = "com.netease.nim_unity_android_demo";
					if (System.IO.Directory.Exists("/sdcard"))
						_appDataPath = Path.Combine("/sdcard", androidPathName);
					else
						_appDataPath = Path.Combine(Application.persistentDataPath, androidPathName);
				}
				else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
				{
					_appDataPath = "NimUnityDemo";
				}
				else
				{
					_appDataPath = Application.persistentDataPath + "/NimUnityDemo";
				}
				Debug.Log("AppDataPath:" + _appDataPath);
			}
			return _appDataPath;
		}
	}

	public void InitChatSystem()
	{
		if (NIM.ClientAPI.Init(AppDataPath))
		{
			Init("10000022", "8de0794f1598c9a484150e7a0d9a9a62");
		}
		else
		{
			Debug.Log("连接聊天失败");
		}
	}

	public void Init(string clientID, string token)
	{
		NIM.ClientAPI.Login(appKey, clientID, token, result =>
		{
			if (result.Code != NIM.ResponseCode.kNIMResSuccess)
			{
				_loginSuccess = false;
				Debug.Log("initFailedDelegate");
				return;
			}

			if (result.LoginStep != NIM.NIMLoginStep.kNIMLoginStepLogin)
			{
				return;
			}
			if (result.Code == NIM.ResponseCode.kNIMResSuccess && result.LoginStep == NIM.NIMLoginStep.kNIMLoginStepLogin)
			{
				_loginSuccess = true;
			}

			Debug.Log("LoginHandler");
			NIMChatRoom.ChatRoomApi.LoginHandler += ChatRoomApi_LoginHandler;
			ConnectChatRoom(10818813);
		});
	}

	public void ConnectChatRoom(long roomId)
	{
		NIM.Plugin.ChatRoom.RequestLoginInfo(roomId, (response, authResult) =>
		{
			Debug.Log("------------------------");
			Debug.Log(response);
			Debug.Log(authResult);

			if (response == NIM.ResponseCode.kNIMResSuccess)
			{
				NIMChatRoom.LoginData data = new NIMChatRoom.LoginData();
				data.Nick = "【C# Client】";
				Debug.Log("Authorize Success");
				Debug.Log("ChatRoom Login");
				NIMChatRoom.ChatRoomApi.Login(roomId, authResult, data);
			}
			else
			{
				Debug.Log("ConnectChatRoom Failed");
			}
		});
	}

	void ChatRoomApi_LoginHandler(NIMChatRoom.NIMChatRoomLoginStep loginStep, NIM.ResponseCode errorCode, NIMChatRoom.ChatRoomInfo roomInfo, NIMChatRoom.MemberInfo memberInfo)
	{
		Debug.Log("ChatRoomApi_LoginHandler called");
	}

	void Awake () {
		InitChatSystem();
	}
}
