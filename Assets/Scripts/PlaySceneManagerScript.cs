using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlaySceneManagerScript : MonoBehaviour
{
    public Button logoutButton;
    
	void Start ()
    {
        logoutButton.onClick.AddListener(onLogoutButtonClicked);
    }

    private void onLogoutButtonClicked()
    {
        SmartFoxInstanceManager.resetEventListeners();
        SmartFoxInstanceManager.disconnect();

        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
    }
}
