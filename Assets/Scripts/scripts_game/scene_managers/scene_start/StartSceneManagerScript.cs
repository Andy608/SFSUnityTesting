using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneManagerScript : MonoBehaviour
{
    public Button loginButton;
    public Button signupButton;

    protected void Start()
    {
        loginButton.onClick.AddListener(onLoginButtonClicked);
        signupButton.onClick.AddListener(onSignupButtonClick);
    }

    private void onLoginButtonClicked()
    {
        SceneManager.LoadScene(SloverseSceneUtil.LOGIN_SCENE);
    }

    private void onSignupButtonClick()
    {
        SceneManager.LoadScene(SloverseSceneUtil.SIGNUP_SCENE);
    }
}
