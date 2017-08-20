using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;

public class SignupSceneManagerScript : MonoBehaviour
{
    public Button signupButton;
    public Button backButton;

    public InputField usernameField;
    public InputField passwordField;
    public InputField emailField;

	protected void Start ()
    {
        signupButton.onClick.AddListener(onSignupButtonClicked);
        backButton.onClick.AddListener(onBackButtonClicked);
	}

    protected void Update()
    {
        if (SmartFoxInstanceManager.connection != null)
        {
            SmartFoxInstanceManager.connection.ProcessEvents();
        }
    }

    private void onSignupButtonClicked()
    {
        if (SmartFoxInstanceManager.connection == null || !SmartFoxInstanceManager.connection.IsConnected)
        {
            ConfigData configData = SmartFoxInstanceManager.generateConfigData();
            SmartFoxInstanceManager.connection.AddEventListener(SFSEvent.EXTENSION_RESPONSE, onExtensionResponse);
            
            SmartFoxInstanceManager.connection.Connect(configData);
        }

        SFSObject signupObj = new SFSObject();
        signupObj.PutUtfString("username", usernameField.text);
        signupObj.PutUtfString("password", passwordField.text);
        signupObj.PutUtfString("email", emailField.text);
        SmartFoxInstanceManager.connection.Send(new ExtensionRequest(SloverseCommandList.SIGNUP_SUBMIT_CMD, signupObj));
    }
	
	private void onBackButtonClicked()
    {
        reset();
        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
    }

    private void onExtensionResponse(BaseEvent e)
    {
        string incomingCommand = (string)e.Params["cmd"];
        ISFSObject commandParams = (ISFSObject)e.Params["params"];

        if (incomingCommand.Equals(SloverseCommandList.SIGNUP_SUBMIT_CMD))
        {
            if (commandParams.GetBool("success"))
            {
                //Go to success page.
                Debug.Log("Woo hoo! You have successfully created an account!");
            }
            else
            {
                Debug.Log("Uh oh. There was an error when creating your account: " + commandParams.GetUtfString("errorMessage"));
            }
        }

        Debug.Log("HELLO");
    }

    private void reset()
    {
        SmartFoxInstanceManager.resetEventListeners();
        SmartFoxInstanceManager.disconnect();

        usernameField.text = "";
        passwordField.text = "";
        emailField.text = "";
    }
}
