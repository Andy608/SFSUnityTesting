using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;

public class ConnectorScript : MonoBehaviour 
{
	public string hostIP = "127.0.0.1";
	public int tcpPort = 9933;
	public int websocketPort = 8888;
	public string zone = "Rain Forest";

	public GameObject loginCanvas;
	public InputField usernameField;
    public InputField passwordField;
	public Button loginButton;
	public Text errorText;

	private SmartFox sfs;

	void Start () 
	{
		errorText.text = "";
		loginButton.onClick.AddListener(OnLoginButtonClick);
	}

	void Update () 
	{
		if (sfs != null)
		{
			sfs.ProcessEvents();
		}
	}

	private void OnLoginButtonClick()
	{
		ConfigData configData = new ConfigData();
		configData.Host = hostIP;

		#if !UNITY_WEBGL
		configData.Port = tcpPort;
		#else
		configData.Port = websocketPort;
		#endif

		configData.Zone = zone;

		#if !UNITY_WEBGL
		sfs = new SmartFox();
		#else
		sfs = new SmartFox(UseWebSocket.WS_BIN);
		#endif

		sfs.ThreadSafeMode = true;

		sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
		sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
		sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
		sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
		sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);

		sfs.Connect(configData);
	}

	private void Reset()
	{
		sfs.RemoveAllEventListeners();
		usernameField.text = "";
        passwordField.text = "";
	}

	private void OnConnection(BaseEvent baseEvent)
	{
		if ((bool)baseEvent.Params["success"])
		{
			SFSConnection.connection = sfs;

			Debug.Log("SFS2X API Version: " + sfs.Version);
			Debug.Log("Connection mode is: " + sfs.ConnectionMode);

			sfs.Send(new Sfs2X.Requests.LoginRequest(usernameField.text, passwordField.text));
		}
		else
		{
			Reset();
			errorText.text = "Could not connect to the server. Is the server running?";
		}
	}

	private void OnConnectionLost(BaseEvent baseEvent)
	{
		Reset();
		string reason = (string) baseEvent.Params["reason"];

		if (reason != ClientDisconnectionReason.MANUAL)
		{
			errorText.text = "Your connection to the server was lost: " + reason;
		}
	}

	private void OnLogin(BaseEvent baseEvent)
	{
		User user = (User) baseEvent.Params["user"];

		Debug.Log("Connection Successful! Logged in as " + user.Name);

		//sfs.Send(new Sfs2X.Requests.JoinRoomRequest("The Lobby"));
	}

	private void OnLoginError(BaseEvent baseEvent)
	{
		sfs.Disconnect();

		Reset();

		errorText.text = "Failed to login: " + (string) baseEvent.Params["errorMessage"];
	}

	private void OnRoomJoin(BaseEvent baseEvent)
	{
		Reset();
		SceneManager.LoadScene("Play");
	}

	private void OnRoomJoinError(BaseEvent baseEvent)
	{
		Debug.Log("Failed to join room: " + (string) baseEvent.Params["errorMessage"]);
	}
}
