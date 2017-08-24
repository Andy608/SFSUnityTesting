using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sfs2X;
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

    private SmartFoxInstanceManager smartFoxManager;

	protected void Start()
    {
        errorText.text = "";
        loginButton.onClick.AddListener(onLoginButtonClicked);
        backButton.onClick.AddListener(onBackButtonClicked);

        smartFoxManager = SmartFoxInstanceManager.getInstance();
    }

    protected void Update()
    {
        if (smartFoxManager.isSmartFoxInitialized())
        {
            smartFoxManager.getSmartFox().ProcessEvents();
        }
    }

    private void onLoginButtonClicked()
    {

        if (!smartFoxManager.isSmartFoxInitialized())
        {
            ConfigData configData = smartFoxManager.generateConfigData();
            smartFoxManager.getSmartFox().AddEventListener(SFSEvent.CONNECTION, onConnection);
            smartFoxManager.getSmartFox().AddEventListener(SFSEvent.CONNECTION_LOST,onConnectionLost);
            smartFoxManager.getSmartFox().AddEventListener(SFSEvent.LOGIN, onLogin);
            smartFoxManager.getSmartFox().AddEventListener(SFSEvent.LOGIN_ERROR, onLoginError);
            smartFoxManager.getSmartFox().AddEventListener(SFSEvent.ROOM_JOIN, onRoomJoin);
            smartFoxManager.getSmartFox().AddEventListener(SFSEvent.ROOM_JOIN_ERROR, onRoomJoinError);

            smartFoxManager.getSmartFox().Connect(configData);
        }
    }

    private void onBackButtonClicked()
    {
        reset();
        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
        smartFoxManager.disconnect();
    }

    private void onConnection(BaseEvent e)
    {
        if ((bool)e.Params["success"])
        {
            smartFoxManager.getSmartFox().Send(new Sfs2X.Requests.LoginRequest(usernameField.text, passwordField.text));
        }
        else
        {
            reset();
            errorText.text = "Uh oh, we're having trouble connecting you to the server right now. Is it running?";
            smartFoxManager.disconnect();
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

        smartFoxManager.disconnect();
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
        smartFoxManager.disconnect();
    }

    private void onRoomJoin(BaseEvent e)
    {
        reset();
        SceneManager.LoadScene(SloverseSceneUtil.PLAY_SCENE);
    }

    private void onRoomJoinError(BaseEvent e)
    {
        Debug.Log("Failed to join room: " + (string)e.Params["errorMessage"]);
    }

    private void reset()
    {
        smartFoxManager.resetEventListeners();

        usernameField.text = "";
        passwordField.text = "";
    }
}
