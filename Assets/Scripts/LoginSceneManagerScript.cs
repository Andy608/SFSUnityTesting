using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;

public class LoginSceneManagerScript : MonoBehaviour
{
    public Button loginButton;
    public Button backButton;

    public InputField usernameField;
    public InputField passwordField;

    public Text errorText;

	protected void Start()
    {
        errorText.text = "";
        loginButton.onClick.AddListener(onLoginButtonClicked);
        backButton.onClick.AddListener(onBackButtonClicked);
    }

    protected void Update()
    {
        if (SmartFoxInstanceManager.connection != null)
        {
            SmartFoxInstanceManager.connection.ProcessEvents();
        }
    }

    private void onLoginButtonClicked()
    {
        if (SmartFoxInstanceManager.connection == null)
        {
            ConfigData configData = SmartFoxInstanceManager.generateConfigData();
            SmartFoxInstanceManager.connection.AddEventListener(SFSEvent.CONNECTION, onConnection);
            SmartFoxInstanceManager.connection.AddEventListener(SFSEvent.CONNECTION_LOST,onConnectionLost);
            SmartFoxInstanceManager.connection.AddEventListener(SFSEvent.LOGIN, onLogin);
            SmartFoxInstanceManager.connection.AddEventListener(SFSEvent.LOGIN_ERROR, onLoginError);
            SmartFoxInstanceManager.connection.AddEventListener(SFSEvent.ROOM_JOIN, onRoomJoin);
            SmartFoxInstanceManager.connection.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, onRoomJoinError);

            SmartFoxInstanceManager.connection.Connect(configData);
        }
    }

    private void onBackButtonClicked()
    {
        reset();
        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
    }

    private void onConnection(BaseEvent e)
    {
        if ((bool)e.Params["success"])
        {
            SmartFoxInstanceManager.connection.Send(new Sfs2X.Requests.LoginRequest(usernameField.text, passwordField.text));
        }
        else
        {
            reset();
            errorText.text = "Uh oh, we're having trouble connecting you to the server right now. Is it running?";
        }
    }

    private void onConnectionLost(BaseEvent e)
    {
        reset();
        string reason = (string)e.Params["reason"];

        if (reason != ClientDisconnectionReason.MANUAL)
        {
            errorText.text = "Connection to the server was lost: " + reason;
        }
    }

    private void onLogin(BaseEvent e)
    {
        User user = (User)e.Params["user"];
        Debug.Log("Connection Successful! Logged in as " + user.Name);
    }

    private void onLoginError(BaseEvent e)
    {
        reset();
        var errorCode = e.Params["errorCode"];

        errorText.text = "Failed to login: The username or password you entered was incorrect.";//(string)e.Params["errorMessage"];
    }

    private void onRoomJoin(BaseEvent e)
    {
        SmartFoxInstanceManager.resetEventListeners();
        SceneManager.LoadScene(SloverseSceneUtil.PLAY_SCENE);
    }

    private void onRoomJoinError(BaseEvent e)
    {
        Debug.Log("Failed to join room: " + (string)e.Params["errorMessage"]);
    }

    private void reset()
    {
        SmartFoxInstanceManager.resetEventListeners();
        SmartFoxInstanceManager.disconnect();

        usernameField.text = "";
        passwordField.text = "";
    }
}
